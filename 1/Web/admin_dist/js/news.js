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
    document.getElementById('NewsImage').value = fileUrl;
}
// load data by Hung
function loaddatabyHung() {
    ref.clear().draw();
    var html = '';
    $.ajax({
        url: "/Base/GetAllNews",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                html = '<a href="#" onclick="showcontent(' + item.NewsId + ');"> Chi tiết </a> | <a href="#" onclick="return getbyID(' + item.NewsId + ');"> Chỉnh sửa</a>';
                htmlimage = '<img src="' + item.NewsImage + '" class="img-responsive">';
                ref.row.add([
                    item.NewsId,
                    item.NewsName,                   
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
// Show content popup hiển thị chi tiết nội dung category đó
function showcontent(id) {
    $.ajax({
        url: "/Base/GetNewsById/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.NewsRemark) {
                $('#modalshowcontent_body').html(result.NewsRemark);
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
// function lấy dữ liệu theo category ID để chỉnh sửa
function getbyID(ID) {
    $('#NewsId').css('border-color', 'lightgrey');
    $('#NewsId').attr('disabled', 'disabled');
    $('#NewsName').css('border-color', 'lightgrey');   
    $('#NewsImage').css('border-color', 'lightgrey');
    $('#NewsDescription').css('border-color', 'lightgrey');
    $('#NewsRemark').css('border-color', 'lightgrey');
    $('#NewsMadeby').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Base/GetNewsById/" + ID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#NewsId').val(result.NewsId);
            $('#NewsName').val(result.NewsName);           
            $('#NewsImage').val(result.NewsImage);
            $('#NewsMadeby').val(result.NewsMadeby);
            CKEDITOR.instances['NewsRemark'].setData(result.NewsRemark);
            CKEDITOR.instances['NewsDescription'].setData(result.NewsDescription);
            $('#myModalLabel').html('<span class="glyphicon glyphicon-envelope"></span> Chỉnh sửa tin');
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
    if ($('#NewsId').val().trim() === "") {
        $('#NewsId').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#NewsId').css('border-color', 'lightgrey');
    }

    if ($('#NewsName').val().trim() === "") {
        $('#NewsName').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#NewsName').css('border-color', 'lightgrey');
    }


    if ($('#NewsImage').val().trim() === "") {
        $('#NewsImage').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#NewsImage').css('border-color', 'lightgrey');
    }
    return isvalidate;
}

// Script cho phan Add: them moi Content
function Add() {   
    var res = validate();
    if (res == false) {
        return false;
    }
    var news =
    {
        NewsId: $('#NewsId').val(),
        NewsName: $('#NewsName').val(),       
        NewsImage: $('#NewsImage').val(),
        NewsDescription: CKEDITOR.instances['NewsDescription'].getData(),
        NewsRemark: CKEDITOR.instances['NewsRemark'].getData(),
        NewsMadeby: $('#NewsMadeby').val()
    };  
    $.ajax({
        url: "/Base/InsertNews",
        data: JSON.stringify(news),
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
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var news =
    {
        NewsId: $('#NewsId').val(),
        NewsName: $('#NewsName').val(),
        NewsImage: $('#NewsImage').val(),
        NewsDescription: CKEDITOR.instances['NewsDescription'].getData(),
        NewsRemark: CKEDITOR.instances['NewsRemark'].getData(),
        NewsMadeby: $('#NewsMadeby').val()
    };
    $.ajax({
        url: "/Base/UpdateNews",
        data: JSON.stringify(news),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function (result) {
            bootbox.alert('Chỉnh sửa thành công!');
            loaddatabyHung();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}
// function clear text box trong modal
function clearTextBox() {
    $('#NewsId').val("");
    $('#NewsName').val("");   
    $('#NewsImage').val("");
    $('#NewsMadeby').val("");
    CKEDITOR.instances['NewsRemark'].setData("")
    CKEDITOR.instances['NewsDescription'].setData("")

    //$('#NewsId').removeAttr('disabled');
    $('#NewsName').removeAttr('disabled');   
    $('#NewsImage').removeAttr('disabled');
    $('#NewsDescription').removeAttr('disabled');
    $('#NewsRemark').removeAttr('disabled');
    $('#NewsMadeby').removeAttr('disabled');
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#NewsName').css('border-color', 'lightgrey');
    $('#NewsRemark').css('border-color', 'lightgrey');
}
// mở popup khi bấm nút add category
function addpopup() {
    $('#myModalLabel').html('<h4><span class="glyphicon glyphicon-envelope"></span> Thêm mới tin</h4>');
    $('#myModal').modal('show');
    clearTextBox();   
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/Base/GetNextNewsId",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#NewsId').val(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
