using System;
using System.Text.RegularExpressions;
using WebApi_BodyMassIndex.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;


    if (Regex.IsMatch(path, "/api/bmi") && request.Method == "GET")
    {

        string mass = request.Query["mass"];
        string height = request.Query["height"];

        await GetBMI(mass, height, response);

    }
    else if (Regex.IsMatch(path, "/api/add_patient") && request.Method == "POST")
    {
        await PostPatient(request, response);

    }
});
app.Run();



async Task GetBMI(string strMass, string strHeight, HttpResponse response)
{
    //Проверка массы и роста на возможность преобразовать в число и на соответствие действительности 
    bool check = СomputeBMI.Checking_MassHeight(strMass, strHeight);//True – значит ошибка

    AnswerBodyMassIndex answerBodyMassIndex = new AnswerBodyMassIndex();

    if (check)
    {
        answerBodyMassIndex.Description = "Error in entered parameters";
        response.StatusCode = 404;
    }
    else
    {
        double mass = double.Parse(strMass);
        double height = double.Parse(strHeight);
        answerBodyMassIndex = СomputeBMI.MeasurementBMI(mass, height); // Расчет индекса массы тела
    }
    response.WriteAsJsonAsync(answerBodyMassIndex);
}

async Task PostPatient(HttpRequest request, HttpResponse response)
{
    string last_name = request.Query["last_name"];
    string first_name = request.Query["first_name"];
    string patronymic = request.Query["patronymic"];

    //Проверка массы и роста на возможность преобразовать в число и на соответствие действительности 
    bool check = СomputeBMI.Checking_MassHeightAge(request.Query["mass"], request.Query["height"], request.Query["age"]);//True – значит ошибка

    if (check)
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Error in entered parameters" });
    }
    else
    {
        double mass = double.Parse(request.Query["mass"]);
        double height = double.Parse(request.Query["height"]);
        int age = int.Parse(request.Query["age"]);

        double massIndex = СomputeBMI.MeasurementBMI(mass, height).BodyMassIndex; // Расчет индекса массы тела

        //Сохранение на базу данных
        await RequestMSSQL.AddPatient(last_name, first_name, patronymic, height, mass, age, massIndex);

    }
}

