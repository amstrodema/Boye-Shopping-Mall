// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
class CartEntry {
    id;

    constructor(id) {
        this.id = id;
    }
}

var cartValue = document.getElementById('cartValue');
var itemTotalLabel = document.getElementById('itemTotalLabel');


function askQuestions() {
    generateLink("Hello Boye Shopping Mall, I'd love to ask a few questions.");
}
function generateLink(message) {

    let number = "+2348060473710";
    //let number = "+2348137395582";
    let url = "https://wa.me/";
    let end_url = `${url}${number}?text=${message}`;

    window.open(end_url, '_blank');

}

CheckCart();

function printCart(id) {
    $.ajax({
        type: "POST",
        url: '/Shop/AddToCart',
        data: JSON.stringify(id),
        contentType: "application/json;",
        success: function (response) {
            alert("Added To Cart");
            CheckCart();
        },
        error: function (response) {
            alert("Try Again Later");
            CheckCart();
        }
    });
}

function RemoveFromCart(id) {
    $.ajax({
        type: "POST",
        url: '/Shop/RemoveFromCart',
        data: JSON.stringify(id),
        contentType: "application/json;",
        success: function (response) {
            alert(response);
            CheckCart();
        },
        error: function (response) {
            alert("Try Again Later");
            CheckCart();
        }
    });
}

function CheckCart() {
    $.ajax({
        type: "GET",
        url: '/Shop/CheckCart',
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response> 0) {
                cartValue.innerText = response
            }
            
        },
        error: function (response) {
            cartValue.innerText = "";
        }
    });
}

function CheckOut(val) {
    generateLink(val)
}

function ItemUp(id, price) {
    var itemDisQty = document.getElementById('itemDisQty');
    var qty = itemDisQty.value * 1;
    price = price * 1;

    qty++;

    var val = qty * price;

    itemTotalLabel.innerText = val;
    printCart(id);
}
function ItemDown(id, price) {
    var itemDisQty = document.getElementById('itemDisQty');

    var val = 0;
    if ((itemDisQty.value - 1) >= 1) {
         val = (itemDisQty.value - 1) * price;
    }

    itemTotalLabel.innerText = val;
    RemoveFromCart(id);
}

function CartItemUp(id, price, lblID) {
    var itemDisQty = document.getElementById(id);
    var lblID = document.getElementById(lblID);
    var cartTotalVal = document.getElementById("cartTotalVal");

    var total = cartTotalVal.innerText * 1;

    itemDisQty.value = (itemDisQty.value*1) + 1;
    var val = itemDisQty.value * price;
    total += price;

    cartTotalVal.innerText = total.toFixed(2);
    lblID.innerText = val;
    printCart(id);
}
function CartItemDown(id, price, lblID) {
    var itemDisQty = document.getElementById(id);
    var lblID = document.getElementById(lblID);
    var cartTotalVal = document.getElementById("cartTotalVal");

    var val = 0;
    var total = cartTotalVal.innerText * 1;

    
    if ((itemDisQty.value) != 0) {
        itemDisQty.value = itemDisQty.value - 1;
    }
    if ((itemDisQty.value) >= 0) {
        val = (itemDisQty.value) * price;
        total -= price;
    }

    cartTotalVal.innerText = total.toFixed(2);
    lblID.innerText = val;
    RemoveFromCart(id);
}