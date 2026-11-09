using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SistemaVentasJoyeria.modulo_proveedor
{
    public partial class index : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var listar = from pvd in db.Proveedor
                         select new
                         {
                             pvd.idProveedor,
                             pvd.codigo,
                             pvd.nombrePvd,
                             pvd.ruc,
                             pvd.correo,
                             pvd.numeroContacto,
                             pvd.direccion,
                             pvd.estado
                         };
            GridView1.DataSource = listar;
            GridView1.DataBind();
        }

        protected void limpiar()
        {
            txtid.Text = "";
            txtcodigo.Text = "";
            txtnom.Text = "";
            txtruc.Text = "";
            txtcel.Text = "";
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

            var proveedor = (from pvd in db.Proveedor where pvd.idProveedor == codigo
                             select pvd).FirstOrDefault();

            txtid.Text = "" + proveedor.idProveedor;
            txtcodigo.Text = proveedor.codigo;
            txtnom.Text = proveedor.nombrePvd;
            txtruc.Text = proveedor.ruc;
            txtcel.Text = proveedor.numeroContacto;
            txtdir.Text = proveedor.direccion;
            txtemail.Text = proveedor.correo;
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
            listar();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "")
            {
                lblmsg.Text = "¡El proveedor ya existe!";
                lblmsgc.Text = "Para registrar debe liberar el campo Código.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || txtruc.Text == "" || txtemail.Text == "" ||
                    txtcel.Text == "" || txtdir.Text == "")
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
                    var valNom = (from p in db.Proveedor where p.nombrePvd == nom select p).FirstOrDefault();
                    if (valNom != null)
                    {
                        lblmsg.Text = "¡En nombre del proveedor ya existe!";
                        lblmsgc.Text = "El nombre ingresado ya está registrado en el sistema.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        String ruc = txtruc.Text;
                        var valruc = (from pr in db.Proveedor where pr.ruc == ruc select pr).FirstOrDefault();
                        if (valruc != null)
                        {
                            lblmsg.Text = "¡En RUC del proveedor ya existe!";
                            lblmsgc.Text = "El RUC ingresado ya está registrado en el sistema.";
                            lblval.Visible = true;
                            msgerror.Attributes.Add("class", "alert alert-danger");
                        }
                        else
                        {
                            Proveedor pvd = new Proveedor();
                            DatosDataContext dbp = new DatosDataContext();
                            pvd.nombrePvd = txtnom.Text;
                            pvd.ruc = txtruc.Text;
                            pvd.correo = txtemail.Text;
                            pvd.numeroContacto = txtcel.Text;
                            pvd.direccion = txtdir.Text;
                            pvd.estado = "Activo";
                            dbp.Proveedor.InsertOnSubmit(pvd);
                            dbp.SubmitChanges();
                            lblval.Visible = false;
                            msgerror.Attributes.Remove("class");
                            listar();
                            limpiar();
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
                lblmsg.Text = "¡Seleccione un proveedor!";
                lblmsgc.Text = "Debe seleccionar un proveedor para poder editar sus datos.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || txtruc.Text == "" || txtemail.Text == "" ||
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
                    var proveedor = (from pvd in db.Proveedor where pvd.idProveedor == id select pvd).FirstOrDefault();

                    proveedor.nombrePvd = txtnom.Text;
                    proveedor.ruc = txtruc.Text;
                    proveedor.correo = txtemail.Text;
                    proveedor.numeroContacto = txtcel.Text;
                    proveedor.direccion = txtdir.Text;
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
                lblmsg.Text = "¡Seleccione un proveedor!";
                lblmsgc.Text = "Debe seleccionar un proveedor para poder activar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var proveedor = (from pvd in db.Proveedor where pvd.idProveedor == id select pvd).FirstOrDefault();
                if (proveedor.estado == "Activo")
                {
                    lblmsg.Text = "¡Proveedor activo!";
                    lblmsgc.Text = "El proveedor seleccionado ya está activado.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    proveedor.estado = "Activo";
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
                lblmsg.Text = "¡Seleccione un proveedor!";
                lblmsgc.Text = "Debe seleccionar un proveedor para poder desactivar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var proveedor = (from pvd in db.Proveedor where pvd.idProveedor == id select pvd).FirstOrDefault();
                if (proveedor.estado == "Inactivo")
                {
                    lblmsg.Text = "¡Proveedor inactivo!";
                    lblmsgc.Text = "El proveedor seleccionado ya está inactivo.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    proveedor.estado = "Inactivo";
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