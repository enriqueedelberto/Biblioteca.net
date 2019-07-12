using BibliotecaDominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        public const string LIBRO_PALINDROMO_SOLO_BIBLIOTECA = "los libros palíndromos solo se pueden utilizar en la biblioteca";

        private  IRepositorioLibro libroRepositorio;
        private  IRepositorioPrestamo prestamoRepositorio;

        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {
         
            //Validar si el Libro existe
            if(!ExisteLibro(isbn) || EsPrestado(isbn)) {
                throw new Exception(EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE);
            }

            if (EsIsbnPalindromo(isbn)) {
                throw new Exception(LIBRO_PALINDROMO_SOLO_BIBLIOTECA);
            }

            var libroPrestar = libroRepositorio.ObtenerPorIsbn(isbn: isbn);
            var prestamo = new Prestamo(
                                  fechaSolicitud: new DateTime(),
                                  libro: libroPrestar, 
                                  fechaEntregaMaxima: new DateTime().AddDays(3), 
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
    }
}
