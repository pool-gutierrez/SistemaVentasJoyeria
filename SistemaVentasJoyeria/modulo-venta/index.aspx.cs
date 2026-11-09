using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Text.RegularExpressions;

namespace SistemaVentasJoyeria.modulo_venta
{
    public partial class index : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var venta = from ven in db.Venta
                         join cli in db.Cliente
                         on ven.idCliente equals cli.idCliente
                         join emp in db.Empleado
                         on ven.idEmpleado equals emp.idEmpleado
                         join tp in db.TipoPago
                         on ven.idPago equals tp.idPago
                         join p in db.Persona
                         on cli.idPersona equals p.idPersona
                         select new
                         {
                             ven.idVenta,
                             ven.codigo,
                             codcli = p.apellidoPaterno+" "+p.apellidoMaterno,
                             codemp = emp.codigo,
                             ven.tipoComprobante,
                             ven.serieComprobante,
                             ven.fechaVenta,
                             ven.horaVenta
                         };
            GridView1.DataSource = venta;
            GridView1.DataBind();
        }

        protected void cargarCli()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from cli in db.Cliente
                           join p in db.Persona
                           on cli.idPersona equals p.idPersona
                           select new
                           {
                               cli.idCliente,
                               campo = String.Format("{0} {1}", p.apellidoPaterno, p.apellidoMaterno)
                           };
            if (IsPostBack == false)
            {
                cmbcli.DataSource = registro;
                cmbcli.DataValueField = "idCliente";
                cmbcli.DataTextField = "campo";
                cmbcli.DataBind();
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
            txtidemp.Text = ""+registro.idEmpleado;
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
            cmbcli.SelectedIndex = 0;
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
            cargarCli();
            cargarEmp();
            cargarTp();
            fecha();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (cmbcli.SelectedIndex == 0 || cmbcom.SelectedIndex == 0 || cmbtp.SelectedIndex == 0)
            {
                lblmsg.Text = "¡Hay campos vacios!";
                lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                Venta ven = new Venta();
                DatosDataContext db = new DatosDataContext();
                ven.idCliente = Convert.ToInt32(cmbcli.SelectedValue);
                ven.idEmpleado = Convert.ToInt32(txtidemp.Text);
                ven.idPago = Convert.ToInt32(cmbtp.SelectedValue);
                ven.tipoComprobante = cmbcom.SelectedItem.Text;
                ven.fechaVenta = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
                ven.horaVenta = TimeSpan.Parse(DateTime.Now.ToString("hh:mm:ss"));
                ven.observaciones = txtob.Text;
                db.Venta.InsertOnSubmit(ven);
                db.SubmitChanges();
                lblval.Visible = false;
                msgerror.Attributes.Remove("class");
                listar();
                limpiar();
                cargarEmp();
                cargarCli();
                cargarTp();
                fecha();
            }
        }

        protected void btnlimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
            listar();
            cargarEmp();
            cargarCli();
            cargarTp();
        }

        protected void btnsesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("/modulo-login/index.aspx");
        }
    }
}