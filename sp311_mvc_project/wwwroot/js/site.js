// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function postRequest(url, data) {
    fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data)
    })
        .then((response) => {
            if (response.ok) {
                window.location.reload();
            }
        })
        .catch((error) => console.error(error));
}

function addToCart(id) {
    postRequest("/Cart/AddToCart", { productId: id });
}

function removeFromCart(id) {
    postRequest("/Cart/RemoveFromCart", { productId: id });
}