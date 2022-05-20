using Nomina.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nomina.ViewModels
{
    public class ProductosViewModel : INotifyPropertyChanged
    {
        FerreteriaContext conext = new();

        private string error;

        public string Errores
        {
            get { return error; }
            set { error = value; Actualizar(""); }
        }

        private Producto producto;

        public Producto Producto
        {
            get { return producto; }
            set { producto = value; Actualizar(""); }
        }

        private ObservableCollection<Producto> productos;

        public ObservableCollection<Producto> Productos
        {
            get { return productos; }
            set { productos = value; Actualizar(""); }
        }

        private List<Seccion> secciones;

        public List<Seccion> Secciones
        {
            get { return secciones; }
            set { secciones = value; }
        }

        private UserControl vista;

        public UserControl Vista
        {
            get { return vista; }
            set { vista = value; }
        }


        //Constructor

        public ProductosViewModel()
        {
            Secciones = new(conext.Seccions.OrderBy(x => x.Nombre));
            ActualizarLista();
        }


        //CRUD  

        //CREATE
        public void VerAgregar()
        {
            //Limpio errores
            Errores = "";
            //Instancio un nuevo producto
            Producto = new();
            //Cambio de ventana
            /////
            //Actualizamos Propiedades
            Actualizar();


        }

        public void Guardar()
        {
            try
            {
                Errores = "";

                if (string.IsNullOrWhiteSpace(Producto.Nombre))
                    Errores = "El nombre no debe ir vacio" + Environment.NewLine;

                if (string.IsNullOrWhiteSpace(Producto.Descripcion))
                    Errores = "La descripcion ir vacia" + Environment.NewLine;

                if (string.IsNullOrWhiteSpace(Producto.Seccion.ToString()))
                    Errores = "La seccion no debe ir vacia" + Environment.NewLine;

                if (Producto.Precio < 0)
                    Errores = "No se admiten valores negativos" + Environment.NewLine;

                if (Errores == "")
                {
                    if (Producto.Id == 0)
                        conext.Add(Producto);
                    else
                        conext.Update(Producto);

                    Errores = "";
                    conext.SaveChanges();                   
                    ActualizarLista();
                    //Debemos regresar vista al inicio
                    //Actualizamos Propiedades
                    Actualizar();
                }
            }
            catch(Exception m)
            {
                Errores = m.Message;
            }

        }

        //READ
        public void ActualizarLista()
        {
            Productos = new(conext.Productos.OrderBy(x => x.Nombre));
        }
        //UPDATE
        public void VerEditar()
        {
            //Limpio errores
            Errores = "";
            //Cambio de ventana
            /////
            //Actualizamos Propiedades
            Actualizar();

        }

        //DELETE
        public void Eliminar()
        {
            Errores = "";
            //Actualizamos Propiedades
            Actualizar();
            //Actualizamos la lista
            ActualizarLista();
        }
        //Metodo para actualizar la lista de productos


        //Metodo para notificar a las propiedades
        public void Actualizar(string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
