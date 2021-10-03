function updatePassword() {
    var password = $("#newPassword").val();
    var confirmPassword = $("#confirmPassword").val();
    var ktp= $("#KTP").val();
    if (password != "" && confirmPassword == password && ktp!="") {
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded",
            url: "/api/auth/updatepassword/",
            data: {
                Password: password,
                KTP : ktp,
            },
            success: function (result) {
                notify('fas fa-check', 'Berhasil', 'Password & KTP berhasil diperbarui', 'success');
            },
            error: function (result) {
                console.log(result);
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    } else {
        notify('fas fa-times', 'Gagal', "KTP kosong atau password dan konfirmasi password tidak sama ", 'danger');
    }
}

function getKTP() {
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/x-www-form-urlencoded",
        url: "/api/auth/getKTP/",
        success: function (result) {
            $("#KTP").val(result);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

$(document).ready(function () {
    getKTP();
});