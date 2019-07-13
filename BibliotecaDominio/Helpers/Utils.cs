using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio.Helpers
{
    public class Utils
    {

        public const string DIAS_NO_NEGATIVAS = "Los días no pueden ser negativos";

        //Método para contar 
        public static DateTime ContarDiasHabiles(DateTime date, int days)
        {
            if (days < 0)
            {
                throw new ArgumentException(DIAS_NO_NEGATIVAS);
            }

            if (days == 0) return date;

           //se calcula la fecha de entrega
            for (int i=1;i<days; i++) {
                date = date.AddDays(1);

                //Si es Domingo, salta al siguiente dia hábil
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(1);

                }

            }

             

            

            return date;

        }
    }
}
