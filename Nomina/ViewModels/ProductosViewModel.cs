using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;
using Nomina.Models;
using Nomina.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nomina.ViewModels
{
    public class ProductosViewModel : INotifyPropertyChanged
    {
        FerreteriaContext conext = new();

        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VerInicioCommand { get; set; }

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
            set { producto = value; Actualizar("Producto"); }
        }

        private ObservableCollection<Producto> productos;

        public ObservableCollection<Producto> Productos
        {
            get { return productos; }
            set { productos = value; Actualizar("Productos"); }
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
            set { vista = value; Actualizar("Vista"); }
        }

        //Ventas del programa

        DetallesUserControl detalles;
        AgregarUserControl agregar;
        EditarUserControl editar;

        public Producto ProductoTemporal { get; set; }

        //Constructor

        public ProductosViewModel()
        {
            AgregarCommand = new RelayCommand(VerAgregar);
            GuardarCommand = new RelayCommand(Guardar);
            VerInicioCommand = new RelayCommand(VerInicio);
            EditarCommand = new RelayCommand(VerEditar);
            EliminarCommand = new RelayCommand(Eliminar);

            detalles = new DetallesUserControl() { DataContext = this };
            Vista = detalles;
            Secciones = new(conext.Seccions.OrderBy(x => x.Nombre));
            ActualizarLista();
        }

        private void VerInicio()
        {
            Errores = "";
            detalles = new() { DataContext = this };
            Vista = detalles;
            Actualizar("");
            ActualizarLista();

            if (producto != null)
                //Esto es para poder guardar el producto anterior en caso de que el cliente quiera cancelar
                if (Producto.Id == 0)
                    Producto = ProductoTemporal;
                else
                    conext.Entry(Producto).Reload();

        }


        //CRUD  

        //CREATE
        public void VerAgregar()
        {
            //Limpio errores
            Errores = "";

            //Creamos un producto temporal por si decide cancelar de ultimo minuto
            ProductoTemporal = producto;

            //Instancio un nuevo producto
            Producto = new();
            //Cambio de ventana
            agregar = new() { DataContext = this };
            Vista = agregar;
            //Actualizamos Propiedades
            Actualizar("");


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


                    //Actualizamos Propiedades
                    Actualizar("Productos");
                    conext.Entry(Producto).Reload();

                    //Debemos regresar vista al inicio
                    VerInicio();
                    ActualizarLista();

                }
            }
            catch (Exception m)
            {
                Errores = m.Message;
            }

        }

        //READ
        public void ActualizarLista()
        {
            Productos = new(conext.Productos.Include(x => x.SeccionNavigation).OrderBy(x => x.Nombre));
            Actualizar("Productos");
        }

        //UPDATE
        public void VerEditar()
        {
            //Limpio errores
            Errores = "";
            //Cambio de ventana
            editar = new() { DataContext = this };
            Vista = editar;
            //Actualizamos Propiedades
            Actualizar("");

        }

        //DELETE
        public void Eliminar()
        {
            var idproductoeliminado = Producto.Id;
            Errores = "";
            //Eliminamos
            conext.Remove(Producto);
            if (conext.SaveChanges() >= 1)
            {
                //Producto = conext.Productos.FirstOrDefault(X => X.Id == (idproductoeliminado + 1));
                Producto = conext.Productos.OrderBy(x => x.Nombre).FirstOrDefault();
            }


            //Actualizamos Propiedades
            Actualizar("");
            //Actualizamos la lista
            ActualizarLista();
        }
        //Metodo para actualizar la lista de productos


        //Metodo para notificar a las propiedades
        public void Actualizar(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
