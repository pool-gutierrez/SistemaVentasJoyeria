using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SistemaVentasJoyeria.modulo_empleado
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
                             per.dni,
                             emp.telefonoMovil,
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
                               campo = String.Format("{0}",rol.nombreRol)
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
            txtid.Text = "";
            txtcodigo.Text = "";
            txtnom.Text = "";
            txtapepat.Text = "";
            txtapemat.Text = "";
            txtdni.Text = "";
            txtfecnac.Text = "";
            txtcel.Text = "";
            txtdir.Text = "";
            txtemail.Text = "";
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
                               per.dni,
                               per.fechaNacimiento,
                               emp.telefonoMovil,
                               emp.direccion,
                               emp.correo,
                               per.usuario,
                               per.contrasena,
                               rol.idRol
                           }).FirstOrDefault();

            txtid.Text = "" + empleado.idEmpleado;
            txtcodigo.Text = empleado.codigo;
            txtnom.Text = empleado.nombrePer;
            txtapepat.Text = empleado.apellidoPaterno;
            txtapemat.Text = empleado.apellidoMaterno;
            txtdni.Text = empleado.dni;
            txtfecnac.Text = String.Format("{0:yyyy-MM-dd}", empleado.fechaNacimiento);
            txtcel.Text = empleado.telefonoMovil;
            txtdir.Text = empleado.direccion;
            txtemail.Text = empleado.correo;
            cmbrol.SelectedValue = empleado.idRol.ToString();
            txtusu.Text = empleado.usuario;
            txtpass.Text = empleado.contrasena;
            txtpassc.Text = empleado.contrasena;
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
            listar();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "")
            {
                lblmsg.Text = "¡El empleado ya existe!";
                lblmsgc.Text = "Para registrar debe liberar el campo Código.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || txtapepat.Text == "" || txtapemat.Text == "" || txtdni.Text == "" || txtfecnac.Text == "" ||
                    txtcel.Text == "" || txtdir.Text == "" || txtemail.Text == "" || cmbrol.SelectedIndex == 0)
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    DatosDataContext db = new DatosDataContext();
                    String dni = txtdni.Text;
                    var valDni = (from p in db.Persona where p.dni == dni select p).FirstOrDefault();
                    if (valDni != null)
                    {
                        lblmsg.Text = "¡En DNI ya existe!";
                        lblmsgc.Text = "El DNI ingresado ya está registrado en el sistema.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        if(txtusu.Text != "")
                        {
                            if(txtpass.Text == ""){
                                lblmsg.Text = "¡Campo contraseña vacio!";
                                lblmsgc.Text = "Si va a registrar un usuario, debe ingresar una contraseña.";
                                lblval.Visible = true;
                                msgerror.Attributes.Add("class", "alert alert-danger");
                            }
                            else
                            {
                                if(txtpass.Text != txtpassc.Text)
                                {
                                    lblmsg.Text = "¡Las contraseñas no coinciden!";
                                    lblmsgc.Text = "La confirmación de contraseña no coincide.";
                                    lblval.Visible = true;
                                    msgerror.Attributes.Add("class", "alert alert-danger");
                                }
                                else
                                {
                                    if(cmbrol.SelectedValue.ToString() == "3"){
                                        lblmsg.Text = "¡Cliente no es un rol válido para un empleado!";
                                        lblmsgc.Text = "Los clientes no tienen acceso al sistema.";
                                        lblval.Visible = true;
                                        msgerror.Attributes.Add("class", "alert alert-danger");
                                    }
                                    else
                                    {
                                        Persona per = new Persona();
                                        Empleado emp = new Empleado();

                                        DatosDataContext dbp = new DatosDataContext();
                                        per.nombrePer = txtnom.Text;
                                        per.apellidoPaterno = txtapepat.Text;
                                        per.apellidoMaterno = txtapemat.Text;
                                        per.dni = txtdni.Text;
                                        per.fechaNacimiento = Convert.ToDateTime(txtfecnac.Text);
                                        per.idRol = Convert.ToInt32(cmbrol.SelectedValue.ToString());
                                        per.usuario = txtusu.Text;
                                        per.contrasena = txtpass.Text;
                                        dbp.Persona.InsertOnSubmit(per);
                                        dbp.SubmitChanges();

                                        var maxPer = (from max in dbp.Persona select max.idPersona).Max();

                                        DatosDataContext dbe = new DatosDataContext();
                                        emp.idPersona = maxPer;
                                        emp.telefonoMovil = txtcel.Text;
                                        emp.direccion = txtdir.Text;
                                        emp.correo = txtemail.Text;
                                        emp.estado = "Activo";
                                        dbe.Empleado.InsertOnSubmit(emp);
                                        dbe.SubmitChanges();
                                        lblval.Visible = false;
                                        msgerror.Attributes.Remove("class");
                                        listar();
                                        limpiar();
                                    }
                                }
                            }
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
                                Persona per = new Persona();
                                Empleado emp = new Empleado();

                                DatosDataContext dbp = new DatosDataContext();
                                per.nombrePer = txtnom.Text;
                                per.apellidoPaterno = txtapepat.Text;
                                per.apellidoMaterno = txtapemat.Text;
                                per.dni = txtdni.Text;
                                per.fechaNacimiento = Convert.ToDateTime(txtfecnac.Text);
                                per.idRol = Convert.ToInt32(cmbrol.SelectedValue.ToString());
                                dbp.Persona.InsertOnSubmit(per);
                                dbp.SubmitChanges();

                                var maxPer = (from max in dbp.Persona select max.idPersona).Max();

                                DatosDataContext dbe = new DatosDataContext();
                                emp.idPersona = maxPer;
                                emp.telefonoMovil = txtcel.Text;
                                emp.direccion = txtdir.Text;
                                emp.correo = txtemail.Text;
                                emp.estado = "Activo";
                                dbe.Empleado.InsertOnSubmit(emp);
                                dbe.SubmitChanges();
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
            if (txtid.Text == "")
            {
                lblmsg.Text = "¡Seleccione un empleado!";
                lblmsgc.Text = "Debe seleccionar un empleado para poder editar sus datos.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || txtapepat.Text == "" || txtapemat.Text == "" || txtdni.Text == "" || txtfecnac.Text == "" ||
                    txtcel.Text == "" || txtdir.Text == "" || txtemail.Text == "" || cmbrol.SelectedIndex == 0)
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    if (txtusu.Text != "")
                    {
                        if (txtpass.Text == "")
                        {
                            lblmsg.Text = "¡Campo contraseña vacio!";
                            lblmsgc.Text = "Si va a actualizar un usuario, debe ingresar una contraseña.";
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
                                int id = Convert.ToInt32(txtid.Text);
                                var empleado = (from emp in db.Empleado where emp.idEmpleado == id select emp).FirstOrDefault();
                                var persona = (from per in db.Persona where per.idPersona == empleado.idPersona select per).FirstOrDefault();

                                persona.nombrePer = txtnom.Text;
                                persona.apellidoPaterno = txtapepat.Text;
                                persona.apellidoMaterno = txtapemat.Text;
                                persona.dni = txtdni.Text;
                                persona.usuario = txtusu.Text;
                                persona.contrasena = txtpass.Text;
                                persona.fechaNacimiento = Convert.ToDateTime(txtfecnac.Text);
                                empleado.telefonoMovil = txtcel.Text;
                                empleado.direccion = txtdir.Text;
                                empleado.correo = txtemail.Text;
                                persona.idRol = Convert.ToInt32(cmbrol.SelectedValue);
                                db.SubmitChanges();
                                lblval.Visible = false;
                                msgerror.Attributes.Remove("class");
                                listar();
                                limpiar();
                            }
                        }
                    }
                    else
                    {
                        int id = Convert.ToInt32(txtid.Text);
                        var empleado = (from emp in db.Empleado where emp.idEmpleado == id select emp).FirstOrDefault();
                        var persona = (from per in db.Persona where per.idPersona == empleado.idPersona select per).FirstOrDefault();

                        persona.nombrePer = txtnom.Text;
                        persona.apellidoPaterno = txtapepat.Text;
                        persona.apellidoMaterno = txtapemat.Text;
                        persona.dni = txtdni.Text;
                        persona.fechaNacimiento = Convert.ToDateTime(txtfecnac.Text);
                        empleado.telefonoMovil = txtcel.Text;
                        empleado.direccion = txtdir.Text;
                        empleado.correo = txtemail.Text;
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

        protected void btnactivar_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            if (txtid.Text == "")
            {
                lblmsg.Text = "¡Seleccione un empleado!";
                lblmsgc.Text = "Debe seleccionar un empleado para poder activar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var empleado = (from emp in db.Empleado where emp.idEmpleado == id select emp).FirstOrDefault();
                if (empleado.estado == "Activo")
                {
                    lblmsg.Text = "¡Empleado activo";
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
            if (txtid.Text == "")
            {
                lblmsg.Text = "¡Seleccione un empleado!";
                lblmsgc.Text = "Debe seleccionar un empleado para poder desactivar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var empleado = (from emp in db.Empleado where emp.idEmpleado == id select emp).FirstOrDefault();
                if (empleado.estado == "Inactivo")
                {
                    lblmsg.Text = "¡Empleado inactivo";
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            listar();
        }

        protected void btnsesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("/modulo-login/index.aspx");
        }
    }
}