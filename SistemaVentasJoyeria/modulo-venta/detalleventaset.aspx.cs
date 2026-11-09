using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Data.SqlClient;
using System.Data;

namespace SistemaVentasJoyeria.modulo_venta
{
    public partial class detalleventaset : System.Web.UI.Page
    {
        protected void listar()
        {
            int id = Convert.ToInt32(Request.Params["id"]);
            DatosDataContext db = new DatosDataContext();
            var listar = from dv in db.DetalleVenta
                         join pro in db.Producto
                         on dv.idProducto equals pro.idProducto
                         join ven in db.Venta
                         on dv.idVenta equals ven.idVenta
                         join cli in db.Cliente
                         on ven.idCliente equals cli.idCliente
                         join p in db.Persona
                         on cli.idPersona equals p.idPersona
                         where dv.idVenta == id
                         select new
                         {
                             dv.idVenta,
                             ven.codigo,
                             cliente = p.apellidoPaterno + " " + p.apellidoMaterno,
                             pro.nombrePro,
                             dv.precioUnitario,
                             dv.cantidad
                         };
            try
            {
                GridView1.DataSource = listar;
                GridView1.DataBind();
            }
            catch (Exception)
            {
                Response.Redirect("/modulo-venta/nuevaventa.aspx");
            }

        }

        protected void cargarPro()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from pro in db.Producto
                           select new
                           {
                               pro.idProducto,
                               campo = String.Format("{0} {1}", pro.nombrePro + " - Stock(", pro.stock + ")")
                           };
            if (IsPostBack == false)
            {
                cmbpro.DataSource = registro;
                cmbpro.DataValueField = "idProducto";
                cmbpro.DataTextField = "campo";
                cmbpro.DataBind();
            }
        }

        protected void cargarVenta()
        {
            DatosDataContext db = new DatosDataContext();
            int id = Convert.ToInt32(Request.Params["id"]);
            var registro = (from ven in db.Venta
                            where ven.idVenta == id
                            join cli in db.Cliente
                            on ven.idCliente equals cli.idCliente
                            join p in db.Persona
                            on cli.idPersona equals p.idPersona
                            select new
                            {
                                ven.idVenta,
                                ven.codigo,
                                clien = p.apellidoPaterno + " " + p.apellidoMaterno
                            }).FirstOrDefault();
            try
            {
                txtid.Text = "" + registro.idVenta;
                txtcodigo.Text = registro.codigo;
                txtcli.Text = registro.clien;
            }
            catch (Exception)
            {
                Response.Redirect("/modulo-venta/nuevaventa.aspx");
            }
        }

        protected void limpiar()
        {
            txtven.Text = "";
            cmbpro.SelectedIndex = 0;
            txtcan.Text = "";
            cargarPro();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = Convert.ToInt32(Session["usuario"].ToString());
                DatosDataContext db = new DatosDataContext();
                var registro = (from emp in db.Empleado
                                join per in db.Persona
                                on emp.idPersona equals per.idPersona
                                where emp.idPersona == idUsuario
                                select new
                                {
                                    per.usuario,
                                    per.nombrePer,
                                    per.apellidoPaterno,
                                    per.apellidoMaterno
                                }).FirstOrDefault();

                lblusuario.Text = registro.usuario;
                lblpersona.Text = registro.apellidoPaterno + " " + registro.apellidoMaterno + " " + registro.nombrePer;
            }
            catch (Exception)
            {
                Response.Redirect("/modulo-login/index.aspx");
            }
            listar();
            cargarPro();
            cargarVenta();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtven.Text == "" || txtcan.Text == "" || cmbpro.SelectedIndex == 0)
            {
                lblmsg.Text = "¡Hay campos vacios!";
                lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                DatosDataContext db = new DatosDataContext();
                int pro = Convert.ToInt32(cmbpro.SelectedValue);
                int stock = Convert.ToInt32(txtcan.Text);
                var valstock = (from p in db.Producto where p.idProducto == pro select p).FirstOrDefault();
                if (valstock.stock < stock)
                {
                    lblmsg.Text = "¡Stock insuficiente!";
                    lblmsgc.Text = "El stock del producto no cubre su solicitud o no hay stock del producto.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    string val1 = txtid.Text;
                    string val2 = cmbpro.SelectedValue;
                    string val3 = txtven.Text;
                    string val4 = txtcan.Text;
                    string cadena = "insert into DetalleVenta values(" + val1 + ",'" + val2 + "','" + val3 + "','" + val4 + "')";
                    string s = System.Configuration.ConfigurationManager.ConnectionStrings["final"].ConnectionString;
                    SqlConnection conexion = new SqlConnection(s);
                    conexion.Open();
                    SqlCommand comando = new SqlCommand(cadena, conexion);
                    comando.ExecuteNonQuery();
                    conexion.Close();

                    int idpro = Convert.ToInt32(cmbpro.SelectedValue);
                    var producto = (from p in db.Producto where p.idProducto == idpro select p).FirstOrDefault();
                    int stokcact = producto.stock - stock;
                    producto.stock = Convert.ToInt32(stokcact);
                    db.SubmitChanges();

                    lblval.Visible = false;
                    msgerror.Attributes.Remove("class");
                    listar();
                    cargarVenta();
                    limpiar();
                }
            }
        }

        protected void btnlimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnvolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("/modulo-venta/nuevaventa.aspx");
        }

        protected void cmbpro_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            int idp = Convert.ToInt32(cmbpro.SelectedValue);

            if (idp == 0)
            {
                txtven.Text = "";
            }
            else
            {
                var registro = (from pro in db.Producto
                                where pro.idProducto == idp
                                select new
                                {
                                    pro.precioVenta
                                }).FirstOrDefault();
                txtven.Text = "" + registro.precioVenta;
            }
        }

        protected void btnsesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("/modulo-login/index.aspx");
        }
    }
}