var ref;
ref = $("#myTable").DataTable({ "scrollX": true });
$(document).ready(function () {
    loaddatabyHung();
});
// load data by Hung
function loaddatabyHung() {
    ref.clear().draw();
    var html = '';
    $.ajax({
        url: "/Base/GetAllCustomer",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                html = '<a href="#" onclick="showcontent(' + item.CustomerId + ');"> Chi tiết </a> | <a href="#" onclick="return getbyID(' + item.CustomerId + ');"> Chỉnh sửa</a> | <a href="#" onclick="Delete(' + item.CustomerId + ');">Xóa</a>';
                htmlimage = '<img src="' + item.CustomerImage + '" class="img-responsive" />';
                ref.row.add([
                    item.CustomerId,
                    item.CustomerName,
                    htmlimage,
                    item.CustomerDescription,
                    html
                ]);
            });
            ref.draw(false);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//ckfinder
function BrowseServer() {
    // You can use the "CKFinder" class to render CKFinder in a page:
    var finder = new CKFinder();
    finder.basePath = '/ckfinder/';	// The path for the installation of CKFinder (default = "/ckfinder/").
    finder.selectActionFunction = SetFileField;
    finder.popup();

    // It can also be done in a single line, calling the "static"
    // popup( basePath, width, height, selectFunction ) function:
    // CKFinder.popup( '../', null, null, SetFileField ) ;
    //
    // The "popup" function can also accept an object as the only argument.
    // CKFinder.popup( { basePath : '../', selectActionFunction : SetFileField } ) ;
}

// This is a sample function which is called when a file is selected in CKFinder.
function SetFileField(fileUrl) {
    document.getElementById('CustomerImage').value = fileUrl;
}
// Show content popup hiển thị chi tiết nội dung customer đó
function showcontent(id) {
    $.ajax({
        url: "/Base/GetCustomerById/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.CustomerRemark) {
                $('#modalshowcontent_body').html(result.CustomerRemark);
            }
            else {
                $('#modalshowcontent_body').html('Không có mô tả');
            }
            $('#modalshowcontent').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
// function lấy dữ liệu theo Customer ID để chỉnh sửa
function getbyID(ID) {
    $('#CustomerId').css('border-color', 'lightgrey');
    $('#CustomerId').attr('disabled', 'disabled');

    $('#CustomerName').css('border-color', 'lightgrey');    
    $('#CustomerImage').css('border-color', 'lightgrey');
    $('#CustomerDescription').css('border-color', 'lightgrey');
    $('#CustomerRemark').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Base/GetCustomerById/" + ID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#CustomerId').val(result.CustomerId);
            $('#CustomerName').val(result.CustomerName);
            $('#CustomerImage').val(result.CustomerImage);
            $('#CustomerDescription').val(result.CustomerDescription);
            CKEDITOR.instances['CustomerRemark'].setData(result.CustomerRemark);

            $('#myModalLabel').html('<span class="glyphicon glyphicon-envelope"></span> Chỉnh sửa');
            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

// function validate khi thêm mới hoặc chỉnh sửa
function validate() {

    var isvalidate = true;    

    if ($('#CustomerName').val().trim() === "") {
        $('#CustomerName').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#CustomerName').css('border-color', 'lightgrey');
    }

    if ($('#CustomerImage').val().trim() === "") {
        $('#CustomerImage').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#CustomerImage').css('border-color', 'lightgrey');
    }

    if ($('#CustomerDescription').val().trim() === "") {
        $('#CustomerDescription').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#CustomerDescription').css('border-color', 'lightgrey');
    }
    return isvalidate;
}

// Script cho phan Add: them moi Content
function Add() {   
    var res = validate();
    if (res == false) {
        return false;
    }
    var customer =
    {
        CustomerId: $('#CustomerId').val(),
        CustomerName: $('#CustomerName').val(),
        CustomerImage: $('#CustomerImage').val(),
        CustomerDescription: $('#CustomerDescription').val(),
        CustomerRemark: CKEDITOR.instances['CustomerRemark'].getData()
    };
    //alert(JSON.stringify(category));
    $.ajax({
        url: "/Base/InsertCustomer",
        data: JSON.stringify(customer),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function (result) {
            bootbox.alert('Thêm thành công!');
            loaddatabyHung();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
// fucntion Update dữ liệu
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var customer =
    {
        CustomerId: $('#CustomerId').val(),
        CustomerName: $('#CustomerName').val(),
        CustomerImage: $('#CustomerImage').val(),
        CustomerDescription: $('#CustomerDescription').val(),
        CustomerRemark: CKEDITOR.instances['CustomerRemark'].getData()
    };
    //alert(JSON.stringify(category));
    $.ajax({
        url: "/Base/UpdateCustomer",
        data: JSON.stringify(customer),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function (result) {
            bootbox.alert('Sửa thành công!');
            loaddatabyHung();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

// function delete ID
function Delete(ID) {
    bootbox.confirm({
        title: "Xóa?",
        message: "Bạn có muốn xóa không?",
        buttons: {
            cancel: {
                label: '<i class="glyphicon glyphicon-remove"></i> Hủy bỏ'
            },
            confirm: {
                label: '<i class="glyphicon glyphicon-ok"></i> Xác nhận'
            }
        },
        callback: function (a) {
            if (a) {
                $.ajax({
                    url: "/Base/DeleteCustomer/" + ID,
                    type: "POST",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        bootbox.alert("Xóa thành công!");
                        loaddatabyHung();
                    },
                    error: function (errormessage) {
                        alert(errormessage.responseText);
                    }
                });
            }
        }
    });
}
// function clear text box trong modal
function clearTextBox() {
    $('#CustomerId').val("");
    $('#CustomerName').val("");
    $('#CustomerImage').val("");   
    $('#CustomerDescription').val("");   
    CKEDITOR.instances['CustomerRemark'].setData("")

    $('#CustomerName').removeAttr('disabled');
    $('#CustomerImage').removeAttr('disabled');
    $('#CustomerDescription').removeAttr('disabled');
    $('#CustomerRemark').removeAttr('disabled');   

    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#CustomerName').css('border-color', 'lightgrey');
    $('#CustomerImage').css('border-color', 'lightgrey');
    $('#CustomerDescription').css('border-color', 'lightgrey');
    $('#CustomerRemark').css('border-color', 'lightgrey');

}
// mở popup khi bấm nút add category
function addpopup() {
    $('#myModalLabel').html('<h4><span class="glyphicon glyphicon-envelope"></span> Thêm mới</h4>');
    $('#myModal').modal('show');
    clearTextBox();
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/Base/GetNextCustomerId",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#CustomerId').val(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


