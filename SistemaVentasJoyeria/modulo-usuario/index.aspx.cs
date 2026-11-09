using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;

namespace SistemaVentasJoyeria.modulo_usuario
{
    public partial class index : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var listar = from emp in db.Empleado
                         join per in db.Persona
                         on emp.idPersona equals per.idPersona
                         join rol in db.Rol
                         on per.idRol equals rol.idRol
                         select new
                         {
                             emp.idEmpleado,
                             emp.codigo,
                             per.nombrePer,
                             per.apellidoPaterno,
                             per.apellidoMaterno,
                             per.usuario,
                             rol.nombreRol,
                             emp.estado
                         };
            GridView1.DataSource = listar;
            GridView1.DataBind();
        }

        protected void cargarRol()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from rol in db.Rol
                           select new
                           {
                               rol.idRol,
                               campo = String.Format("{0}", rol.nombreRol)
                           };
            if (IsPostBack == false)
            {
                cmbrol.DataSource = registro;
                cmbrol.DataValueField = "idRol";
                cmbrol.DataTextField = "campo";
                cmbrol.DataBind();
            }
        }

        protected void limpiar()
        {
            txtidper.Text = "";
            txtidemp.Text = "";
            txtcodigo.Text = "";
            txtusu.Text = "";
            txtpass.Text = "";
            txtpassc.Text = "";
            cmbrol.SelectedIndex = 0;
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
            cargarRol();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            int codigo = Convert.ToInt32(GridView1.SelectedValue.ToString());

            var empleado = (from emp in db.Empleado
                            join per in db.Persona
                            on emp.idPersona equals per.idPersona
                            join rol in db.Rol
                            on per.idRol equals rol.idRol
                            where emp.idEmpleado == codigo
                            select new
                            {
                                emp.idEmpleado,
                                emp.codigo,
                                per.nombrePer,
                                per.apellidoPaterno,
                                per.apellidoMaterno,
                                per.idPersona,
                                per.usuario,
                                per.contrasena,
                                per.idRol
                            }).FirstOrDefault();
            txtidemp.Text = "" + empleado.idEmpleado;
            txtidper.Text = "" + empleado.idPersona;
            txtcodigo.Text = empleado.codigo;
            txtemp.Text = empleado.nombrePer + " " + empleado.apellidoPaterno + " " + empleado.apellidoMaterno;
            txtusu.Text = empleado.usuario;
            txtpass.Text = empleado.contrasena;
            txtpassc.Text = empleado.contrasena;
            cmbrol.SelectedValue = empleado.idRol.ToString();
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
            listar();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            if(txtidper.Text == ""){
                lblmsg.Text = "¡Seleccione un empleado!";
                lblmsgc.Text = "Debe seleccionar un empleado sin usuario para poder registrar su login.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int codigo = Convert.ToInt32(txtidper.Text);
                var valusu = (from p in db.Persona where p.idPersona == codigo select p).FirstOrDefault();
                if (valusu.usuario != null)
                {
                    lblmsg.Text = "¡El empleado ya tiene usuario registrado!";
                    lblmsgc.Text = "No puede registrar nuevamente un usuario para este empleado.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    if (txtusu.Text == "" || txtpass.Text == "" || txtpassc.Text == "" || cmbrol.SelectedIndex == 0)
                    {
                        lblmsg.Text = "¡Hay campos vacios!";
                        lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder agregar el usuario.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        if (txtpass.Text != txtpassc.Text)
                        {
                            lblmsg.Text = "¡Las contraseñas no coinciden!";
                            lblmsgc.Text = "La confirmación de contraseña no coincide.";
                            lblval.Visible = true;
                            msgerror.Attributes.Add("class", "alert alert-danger");
                        }
                        else
                        {
                            if (cmbrol.SelectedValue.ToString() == "3")
                            {
                                lblmsg.Text = "¡Cliente no es un rol válido para un empleado!";
                                lblmsgc.Text = "Los clientes no tienen acceso al sistema.";
                                lblval.Visible = true;
                                msgerror.Attributes.Add("class", "alert alert-danger");
                            }
                            else
                            {
                                var persona = (from per in db.Persona where per.idPersona == codigo select per).FirstOrDefault();
                                persona.usuario = txtusu.Text;
                                persona.contrasena = txtpass.Text;
                                persona.idRol = Convert.ToInt32(cmbrol.SelectedValue);
                                db.SubmitChanges();
                                lblval.Visible = false;
                                msgerror.Attributes.Remove("class");
                                listar();
                                limpiar();
                            }
                        }
                    }
                }
            }
        }

        protected void btneditar_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            if (txtidper.Text == "")
            {
                lblmsg.Text = "¡Seleccione un empleado!";
                lblmsgc.Text = "Debe seleccionar un empleado para poder editar sus datos.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int codigo = Convert.ToInt32(txtidper.Text);
                var valusu = (from p in db.Persona where p.idPersona == codigo select p).FirstOrDefault();
                if (txtusu.Text == "" || txtpass.Text == "" || txtpassc.Text == "" || cmbrol.SelectedIndex == 0)
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder editar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    if (txtpass.Text != txtpassc.Text)
                    {
                        lblmsg.Text = "¡Las contraseñas no coinciden!";
                        lblmsgc.Text = "La confirmación de contraseña no coincide.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        if (cmbrol.SelectedValue.ToString() == "3")
                        {
                            lblmsg.Text = "¡Cliente no es un rol válido para un empleado!";
                            lblmsgc.Text = "Los clientes no tienen acceso al sistema.";
                            lblval.Visible = true;
                            msgerror.Attributes.Add("class", "alert alert-danger");
                        }
                        else
                        {
                            var persona = (from per in db.Persona where per.idPersona == codigo select per).FirstOrDefault();
                            persona.usuario = txtusu.Text;
                            persona.contrasena = txtpass.Text;
                            persona.idRol = Convert.ToInt32(cmbrol.SelectedValue);
                            db.SubmitChanges();
                            lblval.Visible = false;
                            msgerror.Attributes.Remove("class");
                            listar();
                            limpiar();
                        }
                    }
                }
            }
        }

        protected void btnactivar_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            if (txtidemp.Text == "")
            {
                lblmsg.Text = "¡Seleccione un empleado!";
                lblmsgc.Text = "Debe seleccionar un empleado para poder activar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtidemp.Text);
                var empleado = (from emp in db.Empleado where emp.idEmpleado == id select emp).FirstOrDefault();
                if (empleado.estado == "Activo")
                {
                    lblmsg.Text = "¡Empleado activo!";
                    lblmsgc.Text = "El empleado seleccionado ya está activado.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    empleado.estado = "Activo";
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
            if (txtidemp.Text == "")
            {
                lblmsg.Text = "¡Selecciones un empleado!";
                lblmsgc.Text = "Debe seleccionar un empleado para poder desactivar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtidemp.Text);
                var empleado = (from emp in db.Empleado where emp.idEmpleado == id select emp).FirstOrDefault();
                if (empleado.estado == "Inactivo")
                {
                    lblmsg.Text = "¡Empleado inactivo!";
                    lblmsgc.Text = "El empleado seleccionado ya está inactivo.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    empleado.estado = "Inactivo";
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