using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SistemaVentasJoyeria.modulo_cliente
{
    public partial class index : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var listar = from cli in db.Cliente
                         join per in db.Persona
                         on cli.idPersona equals per.idPersona
                         join rol in db.Rol
                         on per.idRol equals rol.idRol
                         select new
                         {
                             cli.idCliente,
                             cli.codigo,
                             per.nombrePer,
                             per.apellidoPaterno,
                             per.apellidoMaterno,
                             per.dni,
                             cli.telefonoMovil,
                             cli.telefonoFijo,
                             rol.nombreRol,
                             cli.estado
                         };
            GridView1.DataSource = listar;
            GridView1.DataBind();
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
            txtfijo.Text = "";
            txtdir.Text = "";
            txtemail.Text = "";
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

            var cliente = (from cli in db.Cliente
                            join per in db.Persona
                            on cli.idPersona equals per.idPersona
                            join rol in db.Rol
                            on per.idRol equals rol.idRol
                            where cli.idCliente == codigo
                            select new
                            {
                                cli.idCliente,
                                cli.codigo,
                                per.nombrePer,
                                per.apellidoPaterno,
                                per.apellidoMaterno,
                                per.dni,
                                per.fechaNacimiento,
                                cli.telefonoMovil,
                                cli.telefonoFijo,
                                cli.direccion,
                                cli.correo
                            }).FirstOrDefault();

            txtid.Text = ""+cliente.idCliente;
            txtcodigo.Text = cliente.codigo;
            txtnom.Text = cliente.nombrePer;
            txtapepat.Text = cliente.apellidoPaterno;
            txtapemat.Text = cliente.apellidoMaterno;
            txtdni.Text = cliente.dni;
            txtfecnac.Text = String.Format("{0:yyyy-MM-dd}", cliente.fechaNacimiento);
            txtcel.Text = cliente.telefonoMovil;
            txtfijo.Text = cliente.telefonoFijo;
            txtdir.Text = cliente.direccion;
            txtemail.Text = cliente.correo;
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
            listar();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "")
            {
                lblmsg.Text = "¡El cliente ya existe!";
                lblmsgc.Text = "Para registrar debe liberar el campo Código.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if(txtnom.Text == "" || txtapepat.Text == "" || txtapemat.Text == "" || txtdni.Text == "" ||txtfecnac.Text == "" || 
                    txtcel.Text == "" || txtdir.Text == ""){
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
                    if(valDni != null){
                        lblmsg.Text = "¡En DNI ya existe!";
                        lblmsgc.Text = "El DNI ingresado ya está registrado en el sistema.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        Persona per = new Persona();
                        Cliente cli = new Cliente();

                        DatosDataContext dbp = new DatosDataContext();
                        per.nombrePer = txtnom.Text;
                        per.apellidoPaterno = txtapepat.Text;
                        per.apellidoMaterno = txtapemat.Text;
                        per.dni = txtdni.Text;
                        per.fechaNacimiento = Convert.ToDateTime(txtfecnac.Text);
                        per.idRol = 3;
                        dbp.Persona.InsertOnSubmit(per);
                        dbp.SubmitChanges();

                        var maxPer = (from max in dbp.Persona select max.idPersona).Max();

                        DatosDataContext dbc = new DatosDataContext();
                        cli.idPersona = maxPer;
                        cli.telefonoMovil = txtcel.Text;
                        cli.telefonoFijo = txtfijo.Text;
                        cli.direccion = txtdir.Text;
                        cli.correo = txtemail.Text;
                        cli.estado = "Activo";
                        dbc.Cliente.InsertOnSubmit(cli);
                        dbc.SubmitChanges();
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
                lblmsg.Text = "¡Seleccione un cliente!";
                lblmsgc.Text = "Debe seleccionar un cliente para poder editar sus datos.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || txtapepat.Text == "" || txtapemat.Text == "" || txtdni.Text == "" || txtfecnac.Text == "" ||
                    txtcel.Text == "" || txtdir.Text == "")
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    int id = Convert.ToInt32(txtid.Text);
                    var cliente = (from cli in db.Cliente where cli.idCliente == id select cli).FirstOrDefault();
                    var persona = (from per in db.Persona where per.idPersona == cliente.idPersona select per).FirstOrDefault();

                    persona.nombrePer = txtnom.Text;
                    persona.apellidoPaterno = txtapepat.Text;
                    persona.apellidoMaterno = txtapemat.Text;
                    persona.dni = txtdni.Text;
                    persona.fechaNacimiento = Convert.ToDateTime(txtfecnac.Text);
                    cliente.telefonoMovil = txtcel.Text;
                    cliente.telefonoFijo = txtfijo.Text;
                    cliente.direccion = txtdir.Text;
                    cliente.correo = txtemail.Text;
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
                lblmsg.Text = "¡Seleccione un cliente!";
                lblmsgc.Text = "Debe seleccionar un cliente para poder activar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var cliente = (from cli in db.Cliente where cli.idCliente == id select cli).FirstOrDefault();
                if(cliente.estado == "Activo"){
                    lblmsg.Text = "¡Cliente activo";
                    lblmsgc.Text = "El cliente seleccionado ya está activado.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    cliente.estado = "Activo";
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
                lblmsg.Text = "¡Seleccione un cliente!";
                lblmsgc.Text = "Debe seleccionar un cliente para poder desactivar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var cliente = (from cli in db.Cliente where cli.idCliente == id select cli).FirstOrDefault();
                if(cliente.estado == "Inactivo"){
                    lblmsg.Text = "¡Cliente inactivo";
                    lblmsgc.Text = "El cliente seleccionado ya está inactivo.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    cliente.estado = "Inactivo";
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