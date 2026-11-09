using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;

namespace SistemaVentasJoyeria.modulo_reportes
{
    public partial class comprobante : System.Web.UI.Page
    {
        protected void cargarCli()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from cli in db.Cliente join p in db.Persona on cli.idPersona equals p.idPersona
                           select new
                           {
                               cli.idCliente,
                               campo = String.Format("{0} {1} {2}", p.apellidoPaterno + " ", p.apellidoMaterno + " ", p.nombrePer)
                           };
            if (IsPostBack == false)
            {
                cmbcli.DataSource = registro;
                cmbcli.DataValueField = "idCliente";
                cmbcli.DataTextField = "campo";
                cmbcli.DataBind();
            }
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
            cargarCli();
        }

        protected void cmbcom_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            string com = cmbcom.SelectedItem.ToString();

            if(cmbcli.SelectedIndex == 0){
                nuevo();
                lblmsg.Text = "¡Seleccione un cliente!";
                lblmsgc.Text = "Debe seleccionar un cliente para ver su comprobante.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if(cmbcom.SelectedIndex == 0){
                    limpiarTodo();
                    lblmsg.Text = "¡Seleccione un tipo de comprobante!";
                    lblmsgc.Text = "Debe seleccionar un tipo de comprobante para ver datos.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    if (com == "Boleta")
                    {
                        lblval.Visible = false;
                        msgerror.Attributes.Remove("class");
                        int cli = Convert.ToInt32(cmbcli.SelectedValue);
                        var boleta = from dt in db.DetalleVenta
                                     join ven in db.Venta
                                     on dt.idVenta equals ven.idVenta
                                     join pro in db.Producto
                                     on dt.idProducto equals pro.idProducto
                                     where ven.idCliente == cli & ven.tipoComprobante == "Boleta"
                                     select new
                                     {
                                         ven.codigo,
                                         ven.serieComprobante,
                                         ven.fechaVenta,
                                         pro.nombrePro,
                                         pro.precioVenta,
                                         dt.cantidad,
                                         monto = dt.precioUnitario * dt.cantidad
                                     };
                        try
                        {
                            GridView1.DataSource = boleta;
                            GridView1.DataBind();
                            decimal subtotal = boleta.Sum(m => m.monto);
                            decimal igv = subtotal * 18 / 100;
                            txtsub.Text = subtotal.ToString("N2");
                            txtigv.Text = igv.ToString("N2");
                            txttot.Text = (subtotal + igv).ToString("N2");
                        }
                        catch (Exception)
                        {
                            limpiarTodo();
                            lblmsg.Text = "¡El cliente no tiene operaciones con boleta!";
                            lblmsgc.Text = "Primero debe tener operaciones con boleta registradas.";
                            lblval.Visible = true;
                            msgerror.Attributes.Add("class", "alert alert-danger");
                        }

                    }
                    else if (com == "Factura")
                    {
                        lblval.Visible = false;
                        msgerror.Attributes.Remove("class");
                        int cli = Convert.ToInt32(cmbcli.SelectedValue);
                        var factura = from dt in db.DetalleVenta
                                      join ven in db.Venta
                                      on dt.idVenta equals ven.idVenta
                                      join pro in db.Producto
                                      on dt.idProducto equals pro.idProducto
                                      where ven.idCliente == cli & ven.tipoComprobante == "Factura"
                                      select new
                                      {
                                          ven.codigo,
                                          ven.serieComprobante,
                                          ven.fechaVenta,
                                          pro.nombrePro,
                                          pro.precioVenta,
                                          dt.cantidad,
                                          monto = dt.precioUnitario * dt.cantidad
                                      };
                        try
                        {
                            GridView1.DataSource = factura;
                            GridView1.DataBind();
                            decimal subtotal = factura.Sum(m => m.monto);
                            decimal igv = subtotal * 18 / 100;
                            txtsub.Text = subtotal.ToString("N2");
                            txtigv.Text = igv.ToString("N2");
                            txttot.Text = (subtotal + igv).ToString("N2");
                        }
                        catch (Exception)
                        {
                            limpiarTodo();
                            lblmsg.Text = "¡El cliente no tiene operaciones con factura!";
                            lblmsgc.Text = "Primero debe tener operaciones con factura registradas.";
                            lblval.Visible = true;
                            msgerror.Attributes.Add("class", "alert alert-danger");
                        }
                    }
                }
            }  
        }

        protected void btnsesion_Click(object sender, EventArgs e)
        {
            Session.Remove("usuario");
            Response.Redirect("/modulo-login/index.aspx");
        }

        protected void cmbcli_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idcli = Convert.ToInt32(cmbcli.SelectedValue);
            DatosDataContext db = new DatosDataContext();
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");

            if(cmbcli.SelectedIndex == 0){
                nuevo();
                lblmsg.Text = "¡Seleccione un cliente!";
                lblmsgc.Text = "Debe seleccionar un cliente para ver su comprobante.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                var clien = (from cl in db.Cliente
                             join p in db.Persona
                             on cl.idPersona equals p.idPersona
                             where cl.idCliente == idcli
                             select new
                             {
                                 cl.codigo,
                                 p.nombrePer,
                                 p.apellidoPaterno,
                                 p.apellidoMaterno,
                                 p.dni,
                                 cl.direccion
                             }).FirstOrDefault();
                txtcodigo.Text = clien.codigo;
                txtcli.Text = clien.apellidoPaterno + " " + clien.apellidoPaterno + " " + clien.apellidoMaterno;
                txtdni.Text = clien.dni;
                txtdir.Text = clien.direccion;
            }
        }

        protected void limpiarTodo()
        {
            txtsub.Text = "";
            txtigv.Text = "";
            txttot.Text = "";
            GridView1.DataSource = null;
        }

        protected void nuevo()
        {
            GridView1.DataSource = null;
            txtcli.Text = "";
            txtdni.Text = "";
            txtdir.Text = "";
            txtcodigo.Text = "";
            txtsub.Text = "";
            txtigv.Text = "";
            txttot.Text = "";
        }
    }
}