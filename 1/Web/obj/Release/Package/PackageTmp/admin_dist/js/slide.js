var ref;
ref = $("#myTable").DataTable({ "scrollX": true });
var button1 = document.getElementById('ckfinder-popup-1');
$(document).ready(function () {
    loaddatabyHung();
});
//ckfinder
function BrowseServer() {
    var finder = new CKFinder();
    finder.basePath = '/ckfinder/';
    finder.selectActionFunction = SetFileField;
    finder.popup();
}
function SetFileField(fileUrl) {
    document.getElementById('SlideImageName').value = fileUrl;
}
// load data by Hung
function loaddatabyHung() {
    ref.clear().draw();
    var html = '';
    $.ajax({
        url: "/Base/GetAllSlideImage",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                html = '<a href="#" onclick="Delete(' + item.SlideId + ');">Xóa</a>';
                htmlimage = '<img src="' + item.SlideImageName + '" class="img-responsive">';
                ref.row.add([
                    item.SlideId,
                    item.SlideImageName,
                    htmlimage,
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
// function lấy dữ liệu theo category ID để chỉnh sửa
//function getbyID(ID) {
//    $('#SlideId').css('border-color', 'lightgrey');
//    $('#SlideId').attr('disabled', 'disabled');
//    $('#SlideImageName').css('border-color', 'lightgrey');   
//    $.ajax({
//        url: "/Base/GetSlideById/" + ID,
//        type: "GET",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            $('#SlideId').val(result.SlideId);
//            $('#SlideImageName').val(result.SlideImageName);
//            $('#myModalLabel').html('<span class="glyphicon glyphicon-envelope"></span> Chỉnh sửa slide');
//            $('#myModal').modal('show');
//            $('#btnUpdate').show();
//            $('#btnAdd').hide();
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });
//}

// function validate khi thêm mới hoặc chỉnh sửa
function validate() {
    var isvalidate = true;
    if ($('#SlideId').val().trim() === "") {
        $('#SlideId').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#SlideId').css('border-color', 'lightgrey');
    }

    if ($('#SlideImageName').val().trim() === "") {
        $('#SlideImageName').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#SlideImageName').css('border-color', 'lightgrey');
    }
    return isvalidate;
}

// Script cho phan Add: them moi Content
function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var slides =
    {
        SlideId: $('#SlideId').val(),
        SlideImageName: $('#SlideImageName').val(),       
    };
    $.ajax({
        url: "/Base/InsertSlideImage",
        data: JSON.stringify(slides),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function (result) {
            bootbox.alert('Thêm mới thành công!');
            loaddatabyHung();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}
// fucntion Update dữ liệu
//function Update() {
//    var res = validate();
//    if (res == false) {
//        return false;
//    }
//    var slides =
//    {
//        SlideId: $('#SlideId').val(),
//        SlideImageName: $('#SlideImageName').val(),      
//    };
//    $.ajax({
//        url: "/Base/UpdateNews",
//        data: JSON.stringify(slides),
//        type: "POST",
//        contentType: "application/json;charset=utf-8",
//        datatype: "json",
//        success: function (result) {
//            bootbox.alert('Chỉnh sửa thành công!');
//            loaddatabyHung();
//            $('#myModal').modal('hide');
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText);
//        }
//    });

//}
// function clear text box trong modal
function clearTextBox() {
    $('#SlideId').val("");
    $('#SlideImageName').val("");   
    $('#SlideImageName').removeAttr('disabled');   
    $('#btnUpdate').hide();
    $('#btnAdd').show();   
}
// mở popup khi bấm nút add category
function addpopup() {
    $('#myModalLabel').html('<h4><span class="glyphicon glyphicon-envelope"></span> Thêm mới slide</h4>');
    $('#myModal').modal('show');
    clearTextBox();
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/Base/GetNextSlideId",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#SlideId').val(result);
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
                    url: "/Base/DeleteSlide/" + ID,
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