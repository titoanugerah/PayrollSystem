var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/position/readDatatable",
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        { "data": "name" },
        { "data": "remark" },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showEditForm('" + row.id + "'); >Edit</button>";
            }
        },
    ]
});


function showAddPositionForm() {
    $('#addPositionModal').modal('show');
}

function showEditForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/position/readDetail/" + id,
        success: function (result) {
            console.log(result);
            $('#editId').val(result.id);
            $('#editName').val(result.name);
            $('#editRemark').val(result.remark);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
    $('#editPositionModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    getDeletedPosition();
}

function addPosition() {
    if ($("#addName").val() != "") {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "JSON",
            contentType: "application/x-www-form-urlencoded",
            url: "api/position/create",
            data: {
                Name: $("#addName").val(),
                Remark: $("#addRemark").val(),
            },
            success: function (result) {
                $("#addName").val("");
                $("#addRemark").val("");
                $('.spinner-border').hide();
                $('#addPositionModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'Position berhasil ditambahkan', 'success');
                reloadTable();
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#addPositionModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    }
    else {
        $('#addPositionModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom nama", 'danger');
    }
}


function updatePosition() {
    if ($("#editName").val() != "") {
        $('.spinner-border').show();
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/x-www-form-urlencoded",
            url: "api/position/update/" + $('#editId').val(),
            data: {
                Name: $("#editName").val(),
                Remark: $("#editRemark").val(),
            },
            success: function (result) {
                $('.spinner-border').hide();
                $('#editPositionModal').modal('hide');
                notify('fas fa-check', 'Berhasil', 'Position berhasil diubah', 'success');
                reloadTable();
            },
            error: function (result) {
                console.log(result);
                $('.spinner-border').hide();
                $('#editPositionModal').modal('hide');
                notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
            }
        });
    }
    else
    {
        $('#editPositionModal').modal('hide');
        notify('fas fa-times', 'Gagal', "Mohon lengkapi kolom nama", 'danger');
    }

}

function deletePosition() {
    $('.delete').show();
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/position/delete/" + $('#editId').val(),
        success: function (result) {
            reloadTable();
            $('.delete').hide();
            $('#editPositionModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Position berhasil dihapus', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.delete').hide();
            $('#editPositionModal').modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function recoverPosition() {
    $('.spinner-border').show();
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        dataType: "JSON",
        url: "api/position/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('.spinner-border').hide();
            $('#addPositionModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'Position berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            $('.spinner-border').hide();
            $('#addPositionModal').modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function getDeletedPosition() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/position/readDeleted",
        success: function (result) {
            var html = "<option value=0> Silahkan Pilih</option>";
            result.forEach(function (data) {
                html = html + "<option value=" + data.id + "> " + data.name + "</option>";
            });
            $('#recoverId').html(html);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.responseText, 'danger');
        }
    });
}

$(document).ready(function () {
    $('.select2addmodal').select2({
        dropdownParent: $('#addPositionModal')
    });
    getDeletedPosition();
    $('.spinner-border').hide();
});
