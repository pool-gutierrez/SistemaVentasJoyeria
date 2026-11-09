using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SistemaVentasJoyeria.modulo_unidmedida
{
    public partial class index : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var listar = from um in db.UnidadMedida
                         select new
                         {
                             um.idUnidadMedida,
                             um.codigo,
                             um.nombreUnid,
                             um.estado
                         };
            GridView1.DataSource = listar;
            GridView1.DataBind();
        }

        protected void limpiar()
        {
            txtid.Text = "";
            txtcodigo.Text = "";
            txtnom.Text = "";
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

            var unid = (from um in db.UnidadMedida
                        where um.idUnidadMedida == codigo
                        select um).FirstOrDefault();

            txtid.Text = "" + unid.idUnidadMedida;
            txtcodigo.Text = unid.codigo;
            txtnom.Text = unid.nombreUnid;
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
            listar();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "")
            {
                lblmsg.Text = "¡La unidad de medida ya existe!";
                lblmsgc.Text = "Para registrar debe liberar el campo Código.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "")
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    DatosDataContext db = new DatosDataContext();
                    String nom = txtnom.Text;
                    var valUm = (from u in db.UnidadMedida where u.nombreUnid == nom select u).FirstOrDefault();
                    if (valUm != null)
                    {
                        lblmsg.Text = "¡La unidad de medida ya existe!";
                        lblmsgc.Text = "La unidad de medida ingresada ya está registrada en el sistema.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        UnidadMedida um = new UnidadMedida();

                        DatosDataContext dbp = new DatosDataContext();
                        um.nombreUnid = txtnom.Text;
                        um.estado = "Activo";
                        dbp.UnidadMedida.InsertOnSubmit(um);
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
                lblmsg.Text = "¡Seleccione una unidad de medida!";
                lblmsgc.Text = "Debe seleccionar una unidad de medida para poder editar sus datos.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "")
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    int id = Convert.ToInt32(txtid.Text);
                    var unid = (from um in db.UnidadMedida where um.idUnidadMedida == id select um).FirstOrDefault();

                    unid.nombreUnid = txtnom.Text;
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
                lblmsg.Text = "¡Seleccione una unidad de medida!";
                lblmsgc.Text = "Debe seleccionar una unidad de medida para poder activar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var unid = (from um in db.UnidadMedida where um.idUnidadMedida == id select um).FirstOrDefault();
                if (unid.estado == "Activo")
                {
                    lblmsg.Text = "¡Unidad de medida activa!";
                    lblmsgc.Text = "La unidad de medida seleccionada ya está activado.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    unid.estado = "Activo";
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
                lblmsg.Text = "¡Seleccione una unidad de medida!";
                lblmsgc.Text = "Debe seleccionar una unidad de medida para poder desactivar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var unid = (from um in db.UnidadMedida where um.idUnidadMedida == id select um).FirstOrDefault();
                if (unid.estado == "Inactivo")
                {
                    lblmsg.Text = "¡Unidad de medida inactivo!";
                    lblmsgc.Text = "La unidad de medida seleccionada ya está inactivo.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    unid.estado = "Inactivo";
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