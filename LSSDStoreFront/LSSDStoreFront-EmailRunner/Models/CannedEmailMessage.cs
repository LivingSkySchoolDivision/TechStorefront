using LSSD.StoreFront.Lib;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSSD.StoreFront.EmailRunner.Models
{
    public class CannedEmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Order Order { get; set; }
        public bool ForCustomer { get; set; }
        public bool ForOrderDesk { get; set; }
               
        public static CannedEmailMessage TestEmail()
        {
            return new CannedEmailMessage()
            {
                Subject = "Test email",
                Content = "<p>This is a <i>test</i> email from the storefront</p>"
            };
        }

        public static CannedEmailMessage CustomerOrderThanks(Order order)
        {
            StringBuilder htmlContent = new StringBuilder();

            htmlContent.Append("<html><head>");
            htmlContent.Append("<style type=\"text/css\">");
            htmlContent.Append("html { font-family: sans-serif; }");
            htmlContent.Append("table { padding: 15px; }");
            htmlContent.Append("td { padding: 5px; }");
            htmlContent.Append(".page_container { padding: 15px; margin: 0 auto; min-width: 300px; max-width: 800px; }");
            htmlContent.Append("table { width: 100%; }");
            htmlContent.Append("th{ text-align: left; border-bottom: 1px solid #C0C0C0; }");
            htmlContent.Append(".footer { margin-top: 20px; color: #909090; text-align: center; border-top: 1px solid #909090; padding: 10px; }");
            htmlContent.Append("h1 { font-size: 16pt; text-align: center; padding: 0; margin: 0; }");
            htmlContent.Append("h2 { text-align: center; border-bottom: 1px solid black; padding: 0; margin: 0; }");
            htmlContent.Append("</style>");
            htmlContent.Append("</head><body><div class=\"page_container\">");
            htmlContent.Append("<h1>Living Sky School Division No. 202</h1>");
            htmlContent.Append("<h2>STOREFRONT ORDER RECEIPT</h2>");
            htmlContent.Append("<p>Thank you for placing an order with the Living Sky Technology Storefront!</p>");
            htmlContent.Append("<p><b>Order Date:</b> " + order.OrderDate.ToLongDateString() + "</p>");
            htmlContent.Append("<p><b>Order ID:</b> <a href=\"https://storefront.lskysd.ca/Order/" + order.OrderThumbprint + " \">" + order.OrderThumbprint + "</a></p>");
            htmlContent.Append("<h3>Items Ordered</h3>");
            htmlContent.Append("<table>");
            htmlContent.Append("<thead><th>Item</th><th>Price</th><th>Quantity</th><th>Total Price (Pre-Tax)</th></thead>");
            foreach (OrderItem item in order.Items)
            {
                htmlContent.Append("<tr><td>" + item.Name + "</td><td>$" + item.ItemBasePrice.ToString("#,##0.00") + "</td><td>" + item.Quantity + "</td><td>$" + item.TotalBasePrice.ToString("#,##0.00") + "</td></tr>");
            }
            htmlContent.Append("</table>");
            htmlContent.Append("<table style=\"width: 300px; margin: auto 0 auto auto; border: 1px solid #C0C0C0;\"><tr>");
            htmlContent.Append("<td><b>Sub Total</b></td><td>$" + order.OrderSubTotal.ToString("#,##0.00") + "</td></tr>");
            htmlContent.Append("<tr><td><b>GST</b></td><td>$" + order.TotalGST.ToString("#,##0.00") + "</td></tr>");
            htmlContent.Append("<tr><td><b>PST</b></td><td>$" + order.TotalPST.ToString("#,##0.00") + "</td></tr>");
            htmlContent.Append("<tr><td><b><a href=\"https://www.recyclemyelectronics.ca/sk/residential/environmental-handling-fee-ehf/\" target=\"_blank\"><abbr title=\"Environmental Handling Fee\">EHF</abbr></a></b></td><td>$" + order.TotalEHF.ToString("#,##0.00") + "</td></tr>");
            htmlContent.Append("<tr><td><big><b>Total</b></big></td><td><big><b>$" + order.OrderGrandTotal.ToString("#,##0.00") + "</b></big></td></tr>");
            htmlContent.Append("</table>");
            htmlContent.Append("<div class=\"footer\">Something not right with your order? Reply to this email to create a ticket in our Help Desk.</div>");
            htmlContent.Append("</div></body></html>");

            return new CannedEmailMessage()
            {
                Subject = "Your LSSD Storefront Order (" + order.OrderDate.ToShortDateString() + ")",
                Content = htmlContent.ToString(),
                ForCustomer = true
            };

        }

        public static CannedEmailMessage OrderDeskNotification(Order order)
        {
            StringBuilder htmlContent = new StringBuilder();

            htmlContent.Append("<html><head><style type=\"text/css\">td {padding: 5px;}</style></head><body>");

            htmlContent.Append("<p><b>Order Date:</b> " + order.OrderDate.ToLongDateString() + "<br/>");
            htmlContent.Append("<b>Order Number:</b> <a href=\"https://storefrontmanager.lskysd.ca/Order/" + order.OrderThumbprint + "\">" + order.OrderThumbprint + "</a><br/>");
            htmlContent.Append("<b>Submitted By:</b> " + order.CustomerFullName + " (" + order.CustomerEmailAddress + ")<br/>");
            htmlContent.Append("<b>Budget Number:</b> " + order.BudgetAccountNumber + "<br/>");
            htmlContent.Append("<b>Total Items:</b> " + order.OrderTotalItems + "<br/>");

            if (!string.IsNullOrEmpty(order.CustomerNotes))
            {
                htmlContent.Append("<p><b>Customer Note:</b> " + order.CustomerNotes + "</p><br/><br/>");
            }

            htmlContent.Append("<table>");
            htmlContent.Append("<thead><th>Item</th><th>Quantity</th><th>Price</th><th>Total Price</th><th>Admin Notes</th></thead>");
            foreach (OrderItem item in order.Items)
            {
                htmlContent.Append("<tr><td><a href=\"https://storefront.lskysd.ca/Item/" + item.ProductId + "\">" + item.Name + "</a></td><td>" + item.Quantity + "</td><td>$" + item.ItemBasePrice.ToString("#,##0.00") + "</td><td>$" + ((decimal)item.Quantity * item.ItemBasePrice).ToString("#,##0.00") + "</td><td></td></tr>");
            }
            htmlContent.Append("</table>");
            htmlContent.Append("&nbsp;");
            htmlContent.Append("<table>");
            htmlContent.Append("<tr><td><b>Sub Total</b></td><td>$" + order.OrderSubTotal.ToString("#,##0.00") + "</td></tr>");
            htmlContent.Append("<tr><td><b>GST</b></td><td>$" + order.TotalGST.ToString("#,##0.00") + "</td></tr>");
            htmlContent.Append("<tr><td><b>PST</b></td><td>$" + order.TotalPST.ToString("#,##0.00") + "</td></tr>");
            htmlContent.Append("<tr><td><b><abbr title=\"Environmental Handling Fee\">EHF</abbr></b></td><td>$" + order.TotalEHF.ToString("#,##0.00") + "</td></tr>");
            htmlContent.Append("<tr><td><big><b>Total</b><big></td><td><big>$" + order.OrderGrandTotal.ToString("#,##0.00") + "</big></td></tr>");
            htmlContent.Append("</table>");
            htmlContent.Append("<p><b>Do not use this ticket to communicate with the customer.</b> Send them an email directly, or create a help desk ticket on their behalf.</p><p>Update order statuses on the <a href=\"https://storefrontmanager.lskysd.ca/Order/" + order.OrderThumbprint + "\">storefront manager page for this order</a>.</p>");

            htmlContent.Append("</div></body></html>");

            return new CannedEmailMessage()
            {
                Subject = "LSSD Storefront Order from " + order.CustomerFullName + " (" + order.OrderDate.ToShortDateString() + ")",
                Content = htmlContent.ToString(),
                Order = order,
                ForOrderDesk = true
            };

        }
    }
}
