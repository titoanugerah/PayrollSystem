var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/payrollDetail/readDatatable/"+$("#payrollHistoryId").val(),
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        { "data": "employee.nik" },
        { "data": "employee.name" },
        {
            "render": function (data, type, row) {
                return "Rp. " + row.takeHomePay.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            }
        },
        { "data": "payrollDetailStatus" },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showDetailForm('" + row.id + "'); >Detail</button>" + "<a href='PayrollDetail/Download/Slip/"+row.id+"' class='btn btn-info' target='_blank'>Download</a>";
            }
        },
    ]
});

function downloadReport() {
    if ($('#districtId').val() == 0) {
        window.open('PayrollHistory/Download/Report/' + $("#payrollHistoryId").val(), '_blank');
    } else {
        window.open('PayrollHistory/Download/Report/' + $("#payrollHistoryId").val() + '/' + $('#districtId').val(), '_blank');
    }
    $('#downloadReportModal').modal('hide');
}

function downloadSlip() {
    window.open('PayrollDetail/Download/Slip/' + $("#id").val() , '_blank');
}

function downloadBankReport() {
    window.open('PayrollHistory/Download/ReportBank/' + $("#payrollHistoryId").val(), '_blank');    
}


function showAddPayrollDetailForm() {
    $('#addPayrollDetailModal').modal('show');
}

function showDownloadReportForm() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/district/read",
        success: function (result) {
            var html = "<option value=0> Semua Distrik </option>";
            result.forEach(function (data) {
                html = html + "<option value=" + data.id + "> " + data.name + "</option>";
            });
            $('#districtId').html(html);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.responseText, 'danger');
        }
    });
    $('#downloadReportModal').modal('show');
}

var formatter = new Intl.NumberFormat('id-ID', {
    style: 'currency',
    currency: 'IDR',
});


function showDetailForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/payrollDetail/readDetail/" + id,
        success: function (result) {
            console.log(result);
            $('#name').val(result.employee.name);
            $('#id').val(result.id);
            $('#nik').val(result.employee.nik);
            $('#mainSalaryBilling').val(formatter.format(result.mainSalaryBilling));
            $('#insentiveBilling').val(formatter.format(result.insentiveBilling));
            $('#attendanceBilling').val(formatter.format(result.attendanceBilling));
            $('#overtimeBilling').val(formatter.format(result.overtimeBilling));
            $('#apreciationBilling').val(formatter.format(result.appreciationBilling));
            $('#bpjsTkDeduction').val(formatter.format(result.bpjsTkDeduction));
            $('#bpjsKesehatanDeduction').val(formatter.format(result.bpjsKesehatanDeduction));
            $('#pensionDeduction').val(formatter.format(result.pensionDeduction));
            $('#pph21').val(formatter.format(result.ppH21));
            $('#anotherDeduction').val(formatter.format(result.anotherDeduction));
            $('#transferFee').val(formatter.format(result.transferFee));
            $('#takeHomePay').val(formatter.format(result.takeHomePay));
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
    $('#payrollDetailModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    notify("fa fa-check", "Berhasil", "Data berhasil di reload", "success");
}

function updatePayrollDetail() {
    var fd = new FormData();
    var files = $('#fileUpload1')[0].files[0];
    fd.append('file', files);
    $.ajax({
        url: 'api/payrollDetail/update/' + $("#payrollHistoryId").val(),
        data: fd,
        processData: false,
        contentType: false,
        type: 'post',
        success: function (response) {
            reloadTable()
            $('#addPayrollDetailModal').modal('hide');
            console.log('success', response);
        },
        error: function (result) {
            console.log('error', result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function submit() {
    $.ajax({
        url: 'api/PayrollHistory/submit/' + $("#payrollHistoryId").val(),
        type: 'post',
        success: function (response) {
            reloadTable()
        },
        error: function (result) {
            console.log('error', result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}


$(document).ready(function () {
    $('.select2addmodal').select2({
        dropdownParent: $('#addPayrollDetailModal')
    });
    $('.select2modal').select2({
        dropdownParent: $('#downloadReportModal')
    });
});
