using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;

namespace SistemaVentasJoyeria.modulo_compra
{
    public partial class nuevacompra : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var compra = from com in db.Compra
                         join pvd in db.Proveedor
                         on com.idProveedor equals pvd.idProveedor
                         join emp in db.Empleado
                         on com.idEmpleado equals emp.idEmpleado
                         join tp in db.TipoPago
                         on com.idPago equals tp.idPago
                         select new
                         {
                             com.idCompra,
                             com.codigo,
                             pvd.nombrePvd,
                             emple = emp.codigo,
                             com.tipoComprobante,
                             com.serieComprobante,
                             com.fechaCompra,
                             com.horaCompra
                         };
            GridView1.DataSource = compra;
            GridView1.DataBind();
        }

        protected void cargarPvd()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from pvd in db.Proveedor
                           select new
                           {
                               pvd.idProveedor,
                               campo = String.Format("{0}", pvd.nombrePvd)
                           };
            if (IsPostBack == false)
            {
                cmbpvd.DataSource = registro;
                cmbpvd.DataValueField = "idProveedor";
                cmbpvd.DataTextField = "campo";
                cmbpvd.DataBind();
            }
        }

        protected void cargarEmp()
        {
            DatosDataContext db = new DatosDataContext();
            int idUsuario = Convert.ToInt32(Session["usuario"].ToString());
            var registro = (from emp in db.Empleado
                            join per in db.Persona
                            on emp.idPersona equals per.idPersona
                            where emp.idPersona == idUsuario
                            select new
                            {
                                emp.idEmpleado
                            }).FirstOrDefault();
            txtidemp.Text = "" + registro.idEmpleado;
            txtemp.Text = lblpersona.Text;
        }

        protected void cargarTp()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from tp in db.TipoPago
                           select new
                           {
                               tp.idPago,
                               campo = String.Format("{0} ", tp.tipo)
                           };
            if (IsPostBack == false)
            {
                cmbtp.DataSource = registro;
                cmbtp.DataValueField = "idPago";
                cmbtp.DataTextField = "campo";
                cmbtp.DataBind();
            }
        }

        protected void limpiar()
        {
            txtfec.Text = "";
            txtob.Text = "";
            cmbpvd.SelectedIndex = 0;
            cmbtp.SelectedIndex = 0;
            cmbcom.SelectedIndex = 0;
            fecha();
            cargarEmp();
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
        }

        protected void fecha()
        {
            txtfec.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
            cargarEmp();
            cargarPvd();
            cargarTp();
            fecha();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (cmbcom.SelectedIndex == 0 || cmbpvd.SelectedIndex == 0 || cmbtp.SelectedIndex == 0)
            {
                lblmsg.Text = "¡Hay campos vacios!";
                lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                Compra com = new Compra();
                DatosDataContext db = new DatosDataContext();
                com.idProveedor = Convert.ToInt32(cmbpvd.SelectedValue);
                com.idEmpleado = Convert.ToInt32(txtidemp.Text);
                com.idPago = Convert.ToInt32(cmbtp.SelectedValue);
                com.tipoComprobante = cmbcom.SelectedItem.Text;
                com.fechaCompra = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                com.horaCompra = TimeSpan.Parse(DateTime.Now.ToString("hh:mm:ss"));
                com.observaciones = txtob.Text;
                db.Compra.InsertOnSubmit(com);
                db.SubmitChanges();
                lblval.Visible = false;
                msgerror.Attributes.Remove("class");
                listar();
                limpiar();
                cargarEmp();
                cargarPvd();
                cargarTp();
                fecha();
            }
        }

        protected void btnlimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
            listar();
            cargarEmp();
            cargarPvd();
            cargarTp();
        }

        protected void btnsesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("/modulo-login/index.aspx");
        }
    }
}