$('.txt-products-quantity').val(1);
$('.btn-add-product-quantity').click(addProductsQuantityToCart);
$('.btn-sub-product-quantity').click(subProductsQuantityToCart);

function addProductsQuantityToCart() {
    let val = $('.txt-products-quantity').val();
    let num = 1;
    if (checkPositiveNumber(val)) num = Math.floor(val) + 1;
    $('.txt-products-quantity').val(num);
}
function subProductsQuantityToCart() {
    let val = $('.txt-products-quantity').val();
    let num = 1;
    if (checkPositiveNumber(val) && Math.floor(val) > 1) {
        num = Math.floor(val) - 1;
    }
    $('.txt-products-quantity').val(num);
}
function checkPositiveNumber(val) {
    if (val == null || val == undefined) return false;
    if (val.trim() == '') return false;
    if (isNaN(val)) return false;
    if (Number(val) <= 0) return false;
    return true;
}

function addItemToCart(bookID) {

    $.ajax({
        type: 'POST',
        url: 'Cart/AddToCart',
        data: { bookID: bookID},
        success: function (data) {
            if (data.result == false) {
                window.location.href = data.redirect;
            }
            if (data.result == true) {
                if (data.maxQuantity == true) {
                    alert("Số lượng sách mua tối đa là 20 quyển sách");
                }
            }
        },
        error: function (ex) {
            alert(ex);
        }
    });
}