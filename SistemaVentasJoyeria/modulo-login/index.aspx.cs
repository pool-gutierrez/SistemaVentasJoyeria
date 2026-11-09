using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;

namespace SistemaVentasJoyeria.modulo_login
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            TextBox1.Focus();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();

            string usu = TextBox1.Text;
            string pass = TextBox2.Text;

            var log = (from d in db.Persona
                       join emple in db.Empleado on d.idPersona equals emple.idPersona
                       join ro in db.Rol on d.idRol equals ro.idRol
                       where d.usuario == usu.Trim() && d.contrasena == pass.Trim()
                       select new
                       {
                           privi = ro.privilegio,
                           emple = emple.estado,
                           idusu = d.idPersona
                       }).FirstOrDefault();

            if (string.IsNullOrEmpty(TextBox1.Text) || string.IsNullOrEmpty(TextBox2.Text))
            {
                Label2.Visible = true;
            }
            else if (log == null)
            {
                Label1.Visible = true;
            }
            else if (log != null)
            {
                Label2.Visible = false;
                Label1.Visible = false;
                Session["privilegio"] = log.privi;
                Session["estado"] = log.emple;
                Session["usuario"] = log.idusu;

                if (Session["estado"].Equals("inactivo"))
                {
                    Label3.Visible = true;
                    // Response.Write("<script language=javascript>alert('Usted no puede ingresar');</script>");
                }
                else
                {
                    Response.Redirect("/modulo-login/verificar.aspx");
                }
            }

            /*
            if (string.IsNullOrEmpty(TextBox1.Text) ||
            string.IsNullOrEmpty(TextBox2.Text))
            {
                Label2.Visible = true;
                Label1.Visible = false;
            } if (log == null)
            {
                Label1.Visible = true;
            }
            if (log != null)
            {
                Label2.Visible = false;
                Label1.Visible = false;
                Session["privilegio"] = log.privi;
                Session["estado"] = log.emple;
                if (Session["estado"].Equals("inactivo"))
                {
                    Response.Write("<script language=javascript>alert('Usted no puede ingresar');</script>");

                }
                else
                {
                    Response.Redirect("../Login/verificar.aspx");
                }
            }
            */
        }
    }
}