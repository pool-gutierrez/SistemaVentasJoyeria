using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Data.SqlClient;
using System.Data;

namespace SistemaVentasJoyeria.modulo_compra
{
    public partial class detallecompraset : System.Web.UI.Page
    {
        protected void listar()
        {
            int id = Convert.ToInt32(Request.Params["id"]);
            DatosDataContext db = new DatosDataContext();
            var listar = from dc in db.DetalleCompra
                         join pro in db.Producto
                         on dc.idProducto equals pro.idProducto
                         join com in db.Compra
                         on dc.idCompra equals com.idCompra
                         join pvd in db.Proveedor
                         on com.idProveedor equals pvd.idProveedor
                         where dc.idCompra == id
                         select new
                         {
                             dc.idCompra,
                             com.codigo,
                             pvd.nombrePvd,
                             pro.nombrePro,
                             dc.precioCompra,
                             dc.cantidad
                         };
            try
            {
                GridView1.DataSource = listar;
                GridView1.DataBind();
            }
            catch (Exception)
            {
                Response.Redirect("/modulo-compra/nuevacompra.aspx");
            }

        }

        protected void cargarPro()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from pro in db.Producto
                           select new
                           {
                               pro.idProducto,
                               campo = String.Format("{0}", pro.nombrePro)
                           };
            if (IsPostBack == false)
            {
                cmbpro.DataSource = registro;
                cmbpro.DataValueField = "idProducto";
                cmbpro.DataTextField = "campo";
                cmbpro.DataBind();
            }
        }

        protected void cargarCompra()
        {
            DatosDataContext db = new DatosDataContext();
            int id = Convert.ToInt32(Request.Params["id"]);
            var registro = (from com in db.Compra
                            where com.idCompra == id
                            join pvd in db.Proveedor
                            on com.idProveedor equals pvd.idProveedor
                            select new
                            {
                                com.idCompra,
                                com.codigo,
                                pvd.nombrePvd
                            }).FirstOrDefault();
            try
            {
                txtid.Text = "" + registro.idCompra;
                txtcodigo.Text = registro.codigo;
                txtpvd.Text = registro.nombrePvd;
            }
            catch
            {
                Response.Redirect("/modulo-compra/nuevacompra.aspx");
            }
        }

        protected void limpiar()
        {
            txtcom.Text = "";
            cmbpro.SelectedIndex = 0;
            txtcan.Text = "";
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
            cargarCompra();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtcom.Text == "" || txtcan.Text == "" || cmbpro.SelectedIndex == 0)
            {
                lblmsg.Text = "¡Hay campos vacios!";
                lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                string val1 = txtid.Text;
                string val2 = cmbpro.SelectedValue;
                string val3 = txtcom.Text;
                string val4 = txtcan.Text;
                string cadena = "insert into DetalleCompra values(" + val1 + ",'" + val2 + "','" + val3 + "','" + val4 + "')";
                string s = System.Configuration.ConfigurationManager.ConnectionStrings["final"].ConnectionString;
                SqlConnection conexion = new SqlConnection(s);
                conexion.Open();
                SqlCommand comando = new SqlCommand(cadena, conexion);
                comando.ExecuteNonQuery();
                conexion.Close();

                DatosDataContext db = new DatosDataContext();
                int stock = Convert.ToInt32(txtcan.Text);
                int idpro = Convert.ToInt32(cmbpro.SelectedValue);
                var producto = (from p in db.Producto where p.idProducto == idpro select p).FirstOrDefault();
                int stokcact = producto.stock + stock;
                producto.stock = Convert.ToInt32(stokcact);
                db.SubmitChanges();

                lblval.Visible = false;
                msgerror.Attributes.Remove("class");
                listar();
                cargarPro();
                cargarCompra();
                limpiar();
            }
        }

        protected void btnlimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnvolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("/modulo-compra/nuevacompra.aspx");
        }

        protected void cmbpro_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            int idp = Convert.ToInt32(cmbpro.SelectedValue);
            if (idp == 0)
            {
                txtcom.Text = "";
            }
            else
            {
                var registro = (from pro in db.Producto
                                where pro.idProducto == idp
                                select new
                                {
                                    pro.precioCompra
                                }).FirstOrDefault();
                txtcom.Text = "" + registro.precioCompra;
            }
        }

        protected void btnsesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("/modulo-login/index.aspx");
        }
    }
}