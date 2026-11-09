using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaVentasJoyeria.modulo_login
{
    public partial class verificar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = Convert.ToInt32(Session["privilegio"].ToString());
            }
            catch (Exception)
            {
                Response.Redirect("/modulo-login/index.aspx");
            }

            try
            {
                if (Session["privilegio"].Equals(1))
                {
                    Response.Redirect("/modulo-main/principal.aspx");
                }
                if (Session["privilegio"].Equals(2))
                {
                    Response.Redirect("/modulo-main/principalset.aspx");
                }
                if (Session["privilegio"].Equals(4))
                {
                    Response.Redirect("/modulo-main/principalget.aspx");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}