using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio.Helpers
{
    public class Utils
    {

       //Método para contar 
        public static DateTime ContarDiasHabiles(DateTime date, int days)
        {
            if (days < 0)
            {
                throw new ArgumentException("days cannot be negative", "days");
            }

            if (days == 0) return date;

           
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
