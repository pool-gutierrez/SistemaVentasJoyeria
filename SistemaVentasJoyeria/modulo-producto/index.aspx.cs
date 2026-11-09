using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaVentasJoyeria.Models;

namespace SistemaVentasJoyeria.modulo_producto
{
    public partial class index : System.Web.UI.Page
    {
        protected void listar()
        {
            DatosDataContext db = new DatosDataContext();
            var listar = from pro in db.Producto
                         join cat in db.Categoria
                         on pro.idCategoria equals cat.idCategoria
                         join pvd in db.Proveedor
                         on pro.idProveedor equals pvd.idProveedor
                         join uni in db.UnidadMedida
                         on pro.idUnidadMedida equals uni.idUnidadMedida
                         select new
                         {
                             pro.idProducto,
                             pro.codigo,
                             pro.nombrePro,
                             cat.nombreCat,
                             pvd.nombrePvd,
                             uni.nombreUnid,
                             pro.precioCompra,
                             pro.precioVenta,
                             pro.stock,
                             pro.estado
                         };
            GridView1.DataSource = listar;
            GridView1.DataBind();
        }

        protected void cargarCat()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from cat in db.Categoria
                           select new
                           {
                               cat.idCategoria,
                               campo = String.Format("{0}", cat.nombreCat)
                           };
            if (IsPostBack == false)
            {
                cmbcat.DataSource = registro;
                cmbcat.DataValueField = "idCategoria";
                cmbcat.DataTextField = "campo";
                cmbcat.DataBind();
            }
        }

        protected void cargarPvd()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from pvd in db.Proveedor
                           select new
                           {
                               pvd.idProveedor,
                               campo = String.Format("{0}", pvd.nombrePvd)
                           };
            if (IsPostBack == false)
            {
                cmbpvd.DataSource = registro;
                cmbpvd.DataValueField = "idProveedor";
                cmbpvd.DataTextField = "campo";
                cmbpvd.DataBind();
            }
        }

        protected void cargarUni()
        {
            DatosDataContext db = new DatosDataContext();
            var registro = from uni in db.UnidadMedida
                           select new
                           {
                               uni.idUnidadMedida,
                               campo = String.Format("{0}", uni.nombreUnid)
                           };
            if (IsPostBack == false)
            {
                cmbuni.DataSource = registro;
                cmbuni.DataValueField = "idUnidadMedida";
                cmbuni.DataTextField = "campo";
                cmbuni.DataBind();
            }
        }

        protected void limpiar()
        {
            txtid.Text = "";
            txtcodigo.Text = "";
            txtnom.Text = "";
            txtdesc.Text = "";
            txtcom.Text = "";
            txtven.Text = "";
            txtsto.Text = "";
            txtcant.Text = "";
            cmbcat.SelectedIndex = 0;
            cmbpvd.SelectedIndex = 0;
            cmbuni.SelectedIndex = 0;
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
            cargarCat();
            cargarPvd();
            cargarUni();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatosDataContext db = new DatosDataContext();
            int codigo = Convert.ToInt32(GridView1.SelectedValue.ToString());

            var producto = (from pro in db.Producto
                            join cat in db.Categoria
                            on pro.idCategoria equals cat.idCategoria
                            join pvd in db.Proveedor
                            on pro.idProveedor equals pvd.idProveedor
                            join uni in db.UnidadMedida
                            on pro.idUnidadMedida equals uni.idUnidadMedida
                            where pro.idProducto == codigo
                            select new
                            {
                                pro.idProducto,
                                pro.codigo,
                                pro.idCategoria,
                                pro.nombrePro,
                                pro.idProveedor,
                                pro.idUnidadMedida,
                                pro.medida,
                                pro.precioCompra,
                                pro.precioVenta,
                                pro.stock,
                                pro.descripcion,
                                pro.estado
                            }).FirstOrDefault();
            txtid.Text = ""+producto.idProducto;
            txtcodigo.Text = producto.codigo;
            cmbcat.SelectedValue = producto.idCategoria.ToString();
            txtnom.Text = producto.nombrePro;
            cmbuni.SelectedValue = producto.idUnidadMedida.ToString();
            txtcant.Text = ""+producto.medida;
            cmbpvd.SelectedValue = producto.idProveedor.ToString();
            txtcom.Text = ""+producto.precioCompra;
            txtven.Text = ""+producto.precioVenta;
            txtsto.Text = ""+producto.stock;
            txtdesc.Text = producto.descripcion;
            lblval.Visible = false;
            msgerror.Attributes.Remove("class");
            listar();
        }

        protected void btnregistrar_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "")
            {
                lblmsg.Text = "¡El producto ya existe!";
                lblmsgc.Text = "Para registrar debe liberar el campo Código.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || txtcant.Text == "" || txtcom.Text == "" || txtven.Text == "" || txtsto.Text == "" 
                    || cmbcat.SelectedIndex == 0 || cmbpvd.SelectedIndex == 0 || cmbuni.SelectedIndex == 0)
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
                    var valnom = (from p in db.Producto where p.nombrePro == nom select p).FirstOrDefault();
                    if (valnom != null)
                    {
                        lblmsg.Text = "¡En producto ya existe!";
                        lblmsgc.Text = "El nombre del producto ingresado ya está registrado en el sistema.";
                        lblval.Visible = true;
                        msgerror.Attributes.Add("class", "alert alert-danger");
                    }
                    else
                    {
                        Producto pro = new Producto();
                        DatosDataContext dbp = new DatosDataContext();
                        pro.idCategoria = Convert.ToInt32(cmbcat.SelectedValue);
                        pro.nombrePro = txtnom.Text;
                        pro.idUnidadMedida = Convert.ToInt32(cmbuni.SelectedValue);
                        pro.medida = Convert.ToInt32(txtcant.Text);
                        pro.idProveedor = Convert.ToInt32(cmbpvd.SelectedValue);
                        pro.precioCompra = Convert.ToDecimal(txtcom.Text);
                        pro.precioVenta = Convert.ToDecimal(txtven.Text);
                        pro.stock = Convert.ToInt32(txtsto.Text);
                        pro.descripcion = txtdesc.Text;
                        pro.estado = "Activo";
                        dbp.Producto.InsertOnSubmit(pro);
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
                lblmsg.Text = "¡Seleccione un producto!";
                lblmsgc.Text = "Debe seleccionar un producto para poder editar sus datos.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                if (txtnom.Text == "" || txtcant.Text == "" || txtcom.Text == "" || txtven.Text == "" || txtsto.Text == ""
                    || cmbcat.SelectedIndex == 0 || cmbpvd.SelectedIndex == 0 || cmbuni.SelectedIndex == 0)
                {
                    lblmsg.Text = "¡Hay campos vacios!";
                    lblmsgc.Text = "Debe completar los campos obligatorios marcados con (*) para poder registrar.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    int id = Convert.ToInt32(txtid.Text);
                    var producto = (from pro in db.Producto where pro.idProducto == id select pro).FirstOrDefault();
                    producto.idCategoria = Convert.ToInt32(cmbcat.SelectedValue);
                    producto.nombrePro = txtnom.Text;
                    producto.idUnidadMedida = Convert.ToInt32(cmbuni.SelectedValue);
                    producto.medida = Convert.ToInt32(txtcant.Text);
                    producto.idProveedor = Convert.ToInt32(cmbpvd.SelectedValue);
                    producto.precioCompra = Convert.ToDecimal(txtcom.Text);
                    producto.precioVenta = Convert.ToDecimal(txtven.Text);
                    producto.stock = Convert.ToInt32(txtsto.Text);
                    producto.descripcion = txtdesc.Text;
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
                lblmsg.Text = "¡Seleccione un producto!";
                lblmsgc.Text = "Debe seleccionar un producto para poder activar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var producto = (from pro in db.Producto where pro.idProducto == id select pro).FirstOrDefault();
                if (producto.estado == "Activo")
                {
                    lblmsg.Text = "¡Producto activo!";
                    lblmsgc.Text = "El producto seleccionado ya está activado.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    producto.estado = "Activo";
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
                lblmsg.Text = "¡Seleccione un producto!";
                lblmsgc.Text = "Debe seleccionar un producto para poder desactivar su estado.";
                lblval.Visible = true;
                msgerror.Attributes.Add("class", "alert alert-danger");
            }
            else
            {
                int id = Convert.ToInt32(txtid.Text);
                var producto = (from pro in db.Producto where pro.idProducto == id select pro).FirstOrDefault();
                if (producto.estado == "Inactivo")
                {
                    lblmsg.Text = "¡Producto inactivo!";
                    lblmsgc.Text = "El producto seleccionado ya está inactivo.";
                    lblval.Visible = true;
                    msgerror.Attributes.Add("class", "alert alert-danger");
                }
                else
                {
                    producto.estado = "Inactivo";
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