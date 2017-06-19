using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Entities.AuxEntities;
using Bukimedia.PrestaSharp.Factories;
using Newtonsoft.Json;
using school.Helpers;
using address = Bukimedia.PrestaSharp.Entities.address;
using @group = Bukimedia.PrestaSharp.Entities.AuxEntities.@group;
using product = Bukimedia.PrestaSharp.Entities.product;

namespace school.Models
{
    public class Prestashop
    {
        private static string baseUrl = "http://elatabaltienda.dged.es/api";
        private static string serviceKey = "MM4X2A7KHDA1VK789QBSX958NZSI9CV4";
        private static string pass = "";
        private static readonly Decimal IVA = (decimal)1.21;

        public static Dictionary<String, object> createUser(string email, string pass, string nombre, string apellidos)
        {
            try
            {
                CustomerFactory userFactory = new CustomerFactory(baseUrl, serviceKey, pass);

                var gCustomer = new group();
                gCustomer.id = 3;
                var gOther = new group();

                //if (extraescolar==1)
                //{
                //    gOther.id = 5;
                //}
                //else
                //{
                    gOther.id = 4;
                //}

                customer user = new customer();

                user.email = email;
                user.passwd = pass;
                user.firstname = nombre;
                user.lastname = apellidos;
                user.active = 1;
                List<@group> grupo = new List<@group>();
                grupo.Add(gCustomer);
                grupo.Add(gOther);
                user.associations.groups = grupo;

                user.id_default_group = gCustomer.id;

                customer c = userFactory.Add(user);

                Dictionary<String, object> res = new Dictionary<string, object>(2);

                res.Add("id", c.id);
                res.Add("secure_key", c.secure_key);

                return res;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        private static long? creteDireccion(long? id_customer, string cif, string nombre, string apellido, string direccion, string cp, string ciudad, string empresa, string telefono)
        {
            AddressFactory aFactory = new AddressFactory(baseUrl, serviceKey, pass);

            address dir = new address();

            dir.id_customer = id_customer;
            dir.alias = "Generada por defecto";
            dir.firstname = nombre;
            dir.lastname = apellido;
            dir.address1 = direccion;
            dir.dni = cif;
            dir.postcode = cp;
            dir.city = ciudad;
            dir.company = empresa;
            dir.phone_mobile = telefono;
            dir.id_country = 6;

            dir = aFactory.Add(dir);

            return dir.id;
        }

        public static void updateUser(long id, string email, string pass, string nombre, string apellidos)
        {
            CustomerFactory userFactory = new CustomerFactory(baseUrl, serviceKey, pass);

            customer u = userFactory.Get(id);

            u.firstname = nombre;
            u.lastname = apellidos;
            u.email = email;
            if (pass != "")
            {
                u.passwd = pass;
            }

            userFactory.Update(u);
        }

        public static void removeUser(long id)
        {
            CustomerFactory userFactory = new CustomerFactory(baseUrl, serviceKey, pass);

            customer u = userFactory.Get(id);

            u.active = 0;

            userFactory.Update(u);
        }

        public static int getCarrito(int idPrestashop, int idusuario, int idcliente, int iddistribuidor)
        {
            List<Dictionary<String, object>> linea_carrito = new List<Dictionary<string, object>>();
            var cartFactory = new CartFactory(baseUrl, serviceKey, pass);

            Dictionary<string, string> filter = new Dictionary<string, string>();

            filter.Add("id_customer", idPrestashop.ToString());
            filter.Add("", idPrestashop.ToString());

            var cart = cartFactory.GetByFilter(filter, "id_customer_DESC", "1");

            if (cart.Count != 0 && cart[0].associations.cart_rows.Count != 0)
            {
                cart c = cart[0];

                var productFactory = new ProductFactory(baseUrl, serviceKey, pass);
                double total = 0.0;

                foreach (cart_row row in cart[0].associations.cart_rows)
                {
                    Dictionary<String, object> linea = new Dictionary<string, object>(5);
                    int cantidad = row.quantity;

                    product product = productFactory.Get(row.id_product.Value);

                    Decimal precioSinIva = Math.Round(product.price / IVA, 2);

                    total += (double)(cantidad * precioSinIva);

                    ProductFactory p = new ProductFactory(baseUrl, serviceKey, pass);

                    product j = p.Get((long)row.id_product);


                    linea.Add("idproducto", row.id_product);
                    linea.Add("referencia", product.reference);
                    linea.Add("precio", precioSinIva);
                    linea.Add("descripcion", product.description[0].Value);
                    linea.Add("nombre", product.name[0].Value);
                    linea.Add("cantidad", row.quantity);
                    linea.Add("id_default_image", j.id_default_image);

                    linea_carrito.Add(linea);
                }
                try
                {
                    cartFactory.Delete(c);
                }
                catch (Exception e)
                {

                }

                return 0;
            }

            return -2;

        }
        public static bool addCarrito(int idPrestashop, string secureKey,List<long> productos)
        {
            try { 
            // cart stuff

            var cartFactory = new CartFactory(baseUrl, serviceKey, pass);
            var cart = new cart();
            cart.id_customer = idPrestashop;
            cart.secure_key = secureKey;
            cart.id_shop = 1;
            cart.id_shop_group = 1;
            cart.id_lang = 1;
            cart.id_currency = 1;
            cart.id_carrier = 1;

            List<cart_row> rows = new List<cart_row>();
            
            foreach (long p in productos)
            {

                cart_row row = new cart_row();
                row.id_product = p;
                row.quantity = 1;

                rows.Add(row);
            }


            cart.associations.cart_rows = rows;

            cart = cartFactory.Add(cart);

            cartFactory.Update(cart);

            return true;
            }
            catch (Exception e)
            {
                Task.Run(async () => { Extensiones.sendTelegram("Mensaje: " + e.Message); }).Wait();
                Task.Run(async () => { Extensiones.sendTelegram("StackTrace: " + e.StackTrace); }).Wait();
            }
            return false;
        }

        public static int createOrder(long? id, long? id_customer, string secure_key, int iddireccion, List<Dictionary<String, int>> lineas, double descuento)
        {
            try
            {
                // cart stuff

                var cartFactory = new CartFactory(baseUrl, serviceKey, pass);
                var cart = new cart();
                cart.id = id;
                cart.id_customer = id_customer;
                cart.secure_key = secure_key;
                cart.id_shop = 1;
                cart.id_shop_group = 1;
                cart.id_address_delivery = cart.id_address_invoice = iddireccion;
                cart.id_lang = 1;
                cart.id_currency = 1;
                cart.id_carrier = 1;

                List<cart_row> rows = new List<cart_row>();
                foreach (Dictionary<string, int> linea in lineas)
                {
                    cart_row row = new cart_row();
                    row.id_address_delivery = iddireccion;
                    row.id_product = linea["idproducto"];
                    row.quantity = linea["cantidad"];

                    rows.Add(row);

                }

                cart.associations.cart_rows = rows;

                cart = cartFactory.Add(cart);

                //Order

                OrderFactory orderFactory = new OrderFactory(baseUrl, serviceKey, pass);
                order order = new order();
                order.id = orderFactory.GetAll().Select(x => x.id).OrderByDescending(x => x).First() + 1;

                order.id_lang = 1;
                order.id_address_delivery = cart.id_address_delivery;
                order.id_address_invoice = cart.id_address_invoice;
                order.id_cart = cart.id;
                order.id_currency = 1;
                order.id_customer = cart.id_customer;
                order.id_carrier = cart.id_carrier;
                order.module = "bankwire";
                order.payment = "Transferencia bancaria";
                order.secure_key = cart.secure_key;
                order.delivery_date = DateTime.Now.AddDays(10.0).ToShortDateString();
                order.current_state = 3;
                order.valid = 1;
                order.conversion_rate = 1.0m;
                order.id_shop = 1;
                order.id_shop_group = 1;
                AssociationsOrder associations = new AssociationsOrder();
                associations.order_rows = new List<order_row>();
                decimal price = 0;

                foreach (cart_row row in cart.associations.cart_rows)
                {
                    associations.order_rows.Add(new order_row
                    {
                        product_id = row.id_product,
                        product_quantity = row.quantity,
                        product_attribute_id = row.id_product_attribute
                    });
                    ;
                }
                order.associations = associations;

                order.total_paid = price;
                order.total_paid_real = price;
                order.total_products = price;
                order.total_products_wt = price;

                order = orderFactory.Add(order);
                if (descuento != 0)
                {
                    order.total_discounts_tax_excl = (decimal)(descuento * -1);
                    order.total_discounts_tax_incl = (decimal)Math.Round(descuento * -1.21, 2);
                    order.total_discounts = (decimal)Math.Round(descuento * -1.21, 2); //El descuento yo lo tengo como negativo y prestashop lo necesita positivo

                    order.total_paid = order.total_paid_tax_incl = order.total_paid - order.total_discounts;

                    order.total_paid_tax_excl = order.total_paid_tax_excl - order.total_discounts_tax_excl;


                    orderFactory.Update(order);
                }
                return 0;

            }
            catch (Exception)
            {
                return -1;
            }


        }


    }
}