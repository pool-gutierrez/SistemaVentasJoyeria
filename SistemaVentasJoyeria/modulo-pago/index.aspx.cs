using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SistemaVentasJoyeria.modulo_pago
{
    public partial class index : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var listar = from tp in db.TipoPago
                         select new
                         {
                             tp.idPago,
                             tp.codigo,
                             tp.tipo,
                             tp.estado
                         };
            GridView1.DataSource = listar;
            GridView1.DataBind();
        }

        protected void limpiar()
        {
            txtid.Text = "";
            txtcodigo.Text = "";
            txttipo.Text = "";
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
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
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            int codigo = Convert.ToInt32(GridView1.SelectedValue.ToString());

            var pago = (from tp in db.TipoPago
                             where tp.idPago == codigo
                             select tp).FirstOrDefault();

            txtid.Text = "" + pago.idPago;
            txtcodigo.Text = pago.codigo;
            txttipo.Text = pago.tipo;
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
            listar();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "")
            {
                lblmsg.Text = "¡El tipo de pago ya existe!";
                lblmsgc.Text = "Para registrar debe liberar el campo Código.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txttipo.Text == "")
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    DatosDataContext db = new DatosDataContext();
                    String nom = txttipo.Text;
                    var valTp = (from t in db.TipoPago where t.tipo == nom select t).FirstOrDefault();
                    if (valTp != null)
                    {
                        lblmsg.Text = "¡El tipo de pago ya existe!";
                        lblmsgc.Text = "El tipo de pago ingresado ya está registrado en el sistema.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        TipoPago tp = new TipoPago();

                        DatosDataContext dbp = new DatosDataContext();
                        tp.tipo = txttipo.Text;
                        tp.estado = "Activo";
                        dbp.TipoPago.InsertOnSubmit(tp);
                        dbp.SubmitChanges();
                        lblval.Visible = false;
                        msgerror.Attributes.Remove("class");
                        listar();
                        limpiar();
                    }
                }
            }
        }

        protected void btneditar_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            if (txtid.Text == "")
            {
                lblmsg.Text = "¡Seleccione un tipo de pago!";
                lblmsgc.Text = "Debe seleccionar un tipo de pago para poder editar sus datos.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txttipo.Text == "")
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    int id = Convert.ToInt32(txtid.Text);
                    var pago = (from tp in db.TipoPago where tp.idPago == id select tp).FirstOrDefault();

                    pago.tipo = txttipo.Text;
                    db.SubmitChanges();
                    lblval.Visible = false;
                    msgerror.Attributes.Remove("class");
                    listar();
                    limpiar();
                }
            }
        }

        protected void btnactivar_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            if (txtid.Text == "")
            {
                lblmsg.Text = "¡Seleccione un tipo de pago!";
                lblmsgc.Text = "Debe seleccionar un tipo de pago para poder activar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var pago = (from tp in db.TipoPago where tp.idPago == id select tp).FirstOrDefault();
                if (pago.estado == "Activo")
                {
                    lblmsg.Text = "¡Tipo de pago activo";
                    lblmsgc.Text = "El tipo de pago seleccionado ya está activado.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    pago.estado = "Activo";
                    db.SubmitChanges();
                    lblval.Visible = false;
                    msgerror.Attributes.Remove("class");
                    listar();
                    limpiar();
                }
            }
        }

        protected void btndesactivar_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            if (txtid.Text == "")
            {
                lblmsg.Text = "¡Seleccione un tipo de pago!";
                lblmsgc.Text = "Debe seleccionar un tipo de pago para poder desactivar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var pago = (from tp in db.TipoPago where tp.idPago == id select tp).FirstOrDefault();
                if (pago.estado == "Inactivo")
                {
                    lblmsg.Text = "¡Tipo de pago inactivo";
                    lblmsgc.Text = "El tipo de pago seleccionado ya está inactivo.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    pago.estado = "Inactivo";
                    db.SubmitChanges();
                    lblval.Visible = false;
                    msgerror.Attributes.Remove("class");
                    listar();
                    limpiar();
                }
            }
        }

        protected void btnlimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        protected void btnsesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("/modulo-login/index.aspx");
        }
    }
}