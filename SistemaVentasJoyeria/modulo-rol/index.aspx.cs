using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SistemaVentasJoyeria.modulo_rol
{
    public partial class index : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var listar = from rol in db.Rol
                         select new
                         {
                             rol.idRol,
                             rol.codigo,
                             rol.nombreRol,
                             rol.descripcion,
                             rol.privilegio,
                             rol.estado
                         };
            GridView1.DataSource = listar;
            GridView1.DataBind();
        }

        protected void limpiar()
        {
            txtid.Text = "";
            txtcodigo.Text = "";
            txtnom.Text = "";
            txtdesc.Text = "";
            cmbpri.SelectedIndex = 0;
            txtnom.Enabled = true;
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

            var rol = (from r in db.Rol
                        where r.idRol == codigo
                        select r).FirstOrDefault();

            if (rol.nombreRol == "Administrador" || rol.nombreRol == "Vendedor" || rol.nombreRol == "Cliente")
            {
                txtnom.Enabled = false;
                txtid.Text = "" + rol.idRol;
                txtcodigo.Text = rol.codigo;
                txtnom.Text = rol.nombreRol;
                txtdesc.Text = rol.descripcion;
                cmbpri.SelectedIndex = rol.privilegio;
                lblval.Visible = false;
                msgerror.Attributes.Remove("class");
                listar();
            }
            else
            {
                txtid.Text = "" + rol.idRol;
                txtcodigo.Text = rol.codigo;
                txtnom.Text = rol.nombreRol;
                txtdesc.Text = rol.descripcion;
                cmbpri.SelectedIndex = rol.privilegio;
                lblval.Visible = false;
                msgerror.Attributes.Remove("class");
                listar();
                txtnom.Enabled = true;
            }
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "")
            {
                lblmsg.Text = "¡El rol ya existe!";
                lblmsgc.Text = "Para registrar debe liberar el campo Código.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || cmbpri.SelectedIndex == 0)
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
                    var valrol = (from r in db.Rol where r.nombreRol== nom select r).FirstOrDefault();
                    if (valrol != null)
                    {
                        lblmsg.Text = "¡En nombre de rol ya existe!";
                        lblmsgc.Text = "El rol ingresado ya está registrado en el sistema.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        Rol rol = new Rol();
                        DatosDataContext dbr = new DatosDataContext();
                        rol.nombreRol = txtnom.Text;
                        rol.descripcion = txtdesc.Text;
                        rol.privilegio = Convert.ToInt32(cmbpri.SelectedValue);
                        rol.estado = "Activo";
                        dbr.Rol.InsertOnSubmit(rol);
                        dbr.SubmitChanges();
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
                lblmsg.Text = "¡Seleccione un rol!";
                lblmsgc.Text = "Debe seleccionar un rol para poder editar sus datos.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || cmbpri.SelectedIndex == 0)
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    if (txtnom.Text == "Administrador")
                    {
                        lblmsg.Text = "¡No puede editar el rol principal del sistema!";
                        lblmsgc.Text = "Si edita el rol administrador podría perder acceso a las funciones administrativas.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        int id = Convert.ToInt32(txtid.Text);
                        var rol = (from r in db.Rol where r.idRol == id select r).FirstOrDefault();

                        rol.nombreRol = txtnom.Text;
                        rol.descripcion = txtdesc.Text;
                        rol.privilegio = Convert.ToInt32(cmbpri.SelectedValue);
                        db.SubmitChanges();
                        lblval.Visible = false;
                        msgerror.Attributes.Remove("class");
                        listar();
                        limpiar();
                    }
                }
            }
        }

        protected void btnactivar_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            if (txtid.Text == "")
            {
                lblmsg.Text = "¡Seleccione un rol!";
                lblmsgc.Text = "Debe seleccionar un rol para poder activar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var rol = (from r in db.Rol where r.idRol == id select r).FirstOrDefault();
                if (rol.estado == "Activo")
                {
                    lblmsg.Text = "¡Rol activo!";
                    lblmsgc.Text = "El rol seleccionado ya está activado.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    rol.estado = "Activo";
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
                lblmsg.Text = "¡Seleccione un rol!";
                lblmsgc.Text = "Debe seleccionar un rol para poder desactivar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var rol = (from r in db.Rol where r.idRol == id select r).FirstOrDefault();
                if (rol.estado == "Inactivo")
                {
                    lblmsg.Text = "¡Rol inactivo!";
                    lblmsgc.Text = "El rol seleccionado ya está inactivo.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    if (txtnom.Text == "Administrador")
                    {
                        lblmsg.Text = "¡No puede desactivar el rol -Administrador-!";
                        lblmsgc.Text = "Si desactiva este rol, perderá acceso al sistema.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        rol.estado = "Inactivo";
                        db.SubmitChanges();
                        lblval.Visible = false;
                        msgerror.Attributes.Remove("class");
                        listar();
                        limpiar();
                    }
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