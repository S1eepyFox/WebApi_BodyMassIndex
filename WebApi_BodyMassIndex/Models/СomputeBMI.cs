namespace WebApi_BodyMassIndex.Models
{
    public class СomputeBMI
    {
        public static bool Checking_MassHeight(string strMass, string strHeight)
        {
            bool boolMass = double.TryParse(strMass, out double mass);
            bool boolHeight = double.TryParse(strHeight, out double height);

            bool check = false;

            if (!boolMass  || !boolHeight) //Проверка на корректность присланных данных 
            {
                check = true;
            }
            else if (mass < ObjectsBMI.minMass || mass > ObjectsBMI.maxMass) //Проверка на реалистичность массы 
            {
                check = true;
            }
            else if (height < ObjectsBMI.minHeight || height > ObjectsBMI.maxHeight) //Проверка на реалистичность роста
            {
                check = true;
            }
            Console.WriteLine(check );
            return check;
        }
        public static bool Checking_MassHeightAge(string strMass, string strHeight, string strAge)
        {
            bool check = Checking_MassHeight(strMass, strHeight);
            bool boolAge = int.TryParse(strAge, out int age);

            if (!boolAge) //Проверка на корректность присланных данных 
            {
                check = true;
            }
            else if (age < ObjectsBMI.minAge || age > ObjectsBMI.maxAge) //Проверка на реалистичность массы 
            {
                check = true;
            }
            Console.WriteLine(check);
            return check;
        }

        public static AnswerBodyMassIndex MeasurementBMI(double mass, double height)
        {
            AnswerBodyMassIndex answerBodyMassIndex = new AnswerBodyMassIndex();

            answerBodyMassIndex.BodyMassIndex = Math.Round(mass / Math.Pow(height / 100, 2));

            for (int i = 0; i < ObjectsBMI.indicatorsBMI.Length; i++) //Сравнение с вычисленного ИМТ с рекомендациями ВОЗ 
            {
                if (answerBodyMassIndex.BodyMassIndex <= ObjectsBMI.indicatorsBMI[i].maxIndex && answerBodyMassIndex.BodyMassIndex > ObjectsBMI.indicatorsBMI[i].minIndex)
                {
                    answerBodyMassIndex.Description = ObjectsBMI.indicatorsBMI[i].description;
                }
            }

            return answerBodyMassIndex;
        }
    }
}
