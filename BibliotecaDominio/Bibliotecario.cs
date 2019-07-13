using BibliotecaDominio.Helpers;
using BibliotecaDominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        public const string LIBRO_PALINDROMO_SOLO_BIBLIOTECA = "los libros palíndromos solo se pueden utilizar en la biblioteca";
        public const int MAX_DIAS_HABILES_PRESTAMO = 15;
        public const int SUMATORIA_DIGITOS_ISBN = 30;

        private IRepositorioLibro libroRepositorio;
        private IRepositorioPrestamo prestamoRepositorio;

        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {

            //Validar si el Libro existe
            if (!ExisteLibro(isbn) || EsPrestado(isbn)) {
                throw new Exception(EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE);
            }

            if (EsIsbnPalindromo(isbn)) {
                throw new Exception(LIBRO_PALINDROMO_SOLO_BIBLIOTECA);
            }

            var libroPrestar = libroRepositorio.ObtenerPorIsbn(isbn: isbn);
            var fechaPrestamo = new DateTime();
            var fechaEntregaMaxima = CalcularFechaEntregaMaxima(isbn: isbn, fechaPrestamo: fechaPrestamo);
            var prestamo = new Prestamo(
                                  fechaSolicitud: fechaPrestamo,
                                  libro: libroPrestar,
                                  fechaEntregaMaxima: fechaEntregaMaxima,
                                  nombreUsuario: nombreUsuario);

            this.prestamoRepositorio.Agregar(prestamo);

        }


        public bool EsPrestado(string isbn)
        {
            var libroPrestado = this.prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);
            if (libroPrestado != null) {
                return true;
            }
            return false;
        }

        public bool ExisteLibro(string isbn) {
            var libroExiste = this.libroRepositorio.ObtenerPorIsbn(isbn);
            if (libroExiste == null) {
                return false;
            }
            return true;
        }

        public bool EsIsbnPalindromo(string isbn) {
            var reverseIsbn = "";
            for (int i = isbn.Length - 1; i >= 0; i--) //String Reverse  
            {
                reverseIsbn += isbn[i].ToString();
            }

            if (reverseIsbn.Equals(isbn)) {
                return true;
            }

            return false;
        }

        //Método para calcular Fecha de Entrega Máxima
        public DateTime? CalcularFechaEntregaMaxima(string isbn, DateTime fechaPrestamo) {
            
            DateTime fechaEntregaMaxima = fechaPrestamo;
            var sum = 0;

            //Se suman los digitos del ISBN
            for (int i = 0; i<isbn.Length; i ++) {
                if (Char.IsDigit(isbn[i])) {
                    sum += Int32.Parse(isbn[i].ToString());
                }
                
            }

            //Se valida si la Suma de digitos de ISBN cumple con la regla de negocios
            if (sum >= SUMATORIA_DIGITOS_ISBN) {
                fechaEntregaMaxima = Utils.ContarDiasHabiles(fechaPrestamo, MAX_DIAS_HABILES_PRESTAMO);


                return fechaEntregaMaxima;
            }

            return null;
        }
    }
}
