var ref;
ref = $("#myTable").DataTable({ "scrollX": true });
var button1 = document.getElementById('ckfinder-popup-1');
$(document).ready(function () {
    loaddatabyHung();
    loadcategoryid();

});
//ckfinder
function BrowseServer1() {
    // You can use the "CKFinder" class to render CKFinder in a page:
    var finder = new CKFinder();
    finder.basePath = '/ckfinder/';	// The path for the installation of CKFinder (default = "/ckfinder/").
    finder.selectActionFunction = SetFileField1;
    finder.popup();

    // It can also be done in a single line, calling the "static"
    // popup( basePath, width, height, selectFunction ) function:
    // CKFinder.popup( '../', null, null, SetFileField ) ;
    //
    // The "popup" function can also accept an object as the only argument.
    // CKFinder.popup( { basePath : '../', selectActionFunction : SetFileField } ) ;
}

// This is a sample function which is called when a file is selected in CKFinder.
function SetFileField1(fileUrl) {
    document.getElementById('Image1').value = fileUrl;
}
function BrowseServer2() {
    // You can use the "CKFinder" class to render CKFinder in a page:
    var finder = new CKFinder();
    finder.basePath = '/ckfinder/';	// The path for the installation of CKFinder (default = "/ckfinder/").
    finder.selectActionFunction = SetFileField2;
    finder.popup();

    // It can also be done in a single line, calling the "static"
    // popup( basePath, width, height, selectFunction ) function:
    // CKFinder.popup( '../', null, null, SetFileField ) ;
    //
    // The "popup" function can also accept an object as the only argument.
    // CKFinder.popup( { basePath : '../', selectActionFunction : SetFileField } ) ;
}

// This is a sample function which is called when a file is selected in CKFinder.
function SetFileField2(fileUrl) {
    document.getElementById('Image2').value = fileUrl;
}
function BrowseServer3() {
    // You can use the "CKFinder" class to render CKFinder in a page:
    var finder = new CKFinder();
    finder.basePath = '/ckfinder/';	// The path for the installation of CKFinder (default = "/ckfinder/").
    finder.selectActionFunction = SetFileField3;
    finder.popup();

    // It can also be done in a single line, calling the "static"
    // popup( basePath, width, height, selectFunction ) function:
    // CKFinder.popup( '../', null, null, SetFileField ) ;
    //
    // The "popup" function can also accept an object as the only argument.
    // CKFinder.popup( { basePath : '../', selectActionFunction : SetFileField } ) ;
}

// This is a sample function which is called when a file is selected in CKFinder.
function SetFileField3(fileUrl) {
    document.getElementById('Image3').value = fileUrl;
}
function BrowseServer4() {
    // You can use the "CKFinder" class to render CKFinder in a page:
    var finder = new CKFinder();
    finder.basePath = '/ckfinder/';	// The path for the installation of CKFinder (default = "/ckfinder/").
    finder.selectActionFunction = SetFileField4;
    finder.popup();

    // It can also be done in a single line, calling the "static"
    // popup( basePath, width, height, selectFunction ) function:
    // CKFinder.popup( '../', null, null, SetFileField ) ;
    //
    // The "popup" function can also accept an object as the only argument.
    // CKFinder.popup( { basePath : '../', selectActionFunction : SetFileField } ) ;
}

// This is a sample function which is called when a file is selected in CKFinder.
function SetFileField4(fileUrl) {
    document.getElementById('Image4').value = fileUrl;
}
// load data by Hung
function loaddatabyHung() {
    ref.clear().draw();
    var html = '';
    $.ajax({
        url: "/Base/GetAllProduct",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                html = '<a href="#" onclick="showcontent(' + item.Id + ');"> Chi tiết </a> | <a href="#" onclick="return getbyID(' + item.Id + ');"> Sửa</a> | <a href="#" onclick="Delete(' + item.Id + ');">Xóa</a>';
                htmlimage = '<img src="' + item.Image.split('|')[0] + '" class="img-responsive">';
                ref.row.add([
                    item.Id,
                    item.Name,
                    item.MadeFrom,
                    item.CategoryId,
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
function loadcategoryid() {
    $.ajax({
        url: "/Base/GetAllCategory",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<option value="' + item.CategoryId + '">' + item.CategoryName + '</option>';
            });
            $("#CategoryId").html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
// Show content popup hiển thị chi tiết nội dung category đó
function showcontent(id) {
    $.ajax({
        url: "/Base/GetProductById/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Remark) {
                $('#modalshowcontent_body').html(result.Remark);
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
    loadcategoryid();
    $('#Id').css('border-color', 'lightgrey');
    $('#Id').attr('disabled', 'disabled');
    $('#Name').css('border-color', 'lightgrey');
    $('#MadeFrom').css('border-color', 'lightgrey');
    $('#CategoryId').css('border-color', 'lightgrey');
    //$('#Dimenson').css('border-color', 'lightgrey');
    $('#Quantity').css('border-color', 'lightgrey');
    $('#Value').css('border-color', 'lightgrey');
    $('#Image1').css('border-color', 'lightgrey');
    $('#Image2').css('border-color', 'lightgrey');
    $('#Image3').css('border-color', 'lightgrey');
    $('#Image4').css('border-color', 'lightgrey');
    $('#Seo').css('border-color', 'lightgrey');
    $('#Remark').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Base/GetProductById/" + ID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Image != null) {
                if (result.Image.includes("|")) {
                    var lstimg = result.Image.split("|");
                    var count = lstimg.length;
                    switch (count) {
                        case 1:
                            {
                                $('#Image1').val(lstimg[0]);
                                $('#Image2').val("");
                                $('#Image3').val("");
                                $('#Image4').val("");
                                break;
                            }
                        case 2:
                            {
                                $('#Image1').val(lstimg[0]);
                                $('#Image2').val(lstimg[1]);
                                $('#Image3').val("");
                                $('#Image4').val("");
                                break;
                            }
                        case 3:
                            {
                                $('#Image1').val(lstimg[0]);
                                $('#Image2').val(lstimg[1]);
                                $('#Image3').val(lstimg[2]);
                                $('#Image4').val("");
                                break;
                            }
                        case 4:
                            {
                                $('#Image1').val(lstimg[0]);
                                $('#Image2').val(lstimg[1]);
                                $('#Image3').val(lstimg[2]);
                                $('#Image4').val(lstimg[3]);
                                break;
                            }
                    }
                }
                else {
                    $('#Image1').val(result.Image);
                    $('#Image2').val("");
                    $('#Image3').val("");
                    $('#Image4').val("");
                }
            }
            $('#Id').val(result.Id);
            $('#Name').val(result.Name);
            $('#MadeFrom').val(result.MadeFrom);
            $('#Seo').val(result.Seo);
            //$('#CategoryId').find('option').remove().end();
            //$('#CategoryId').append('<option value="' + result.CategoryId + '">' + result.CategoryId + '</option>');
            $('#CategoryId option[value=' + result.CategoryId + ']').attr('selected', 'selected');
            $('#Quantity').val(result.Quantity);
            $('#Value').val(result.Value);
            $('#Tag').val(result.Tag);
            CKEDITOR.instances['Remark'].setData(result.Remark);
            $('#myModalLabel').html('<span class="glyphicon glyphicon-envelope"></span> Chỉnh sửa sản phẩm');
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
    if ($('#Id').val().trim() === "") {
        $('#Id').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Id').css('border-color', 'lightgrey');
    }

    if ($('#Name').val().trim() === "") {
        $('#Name').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }


    if ($('#MadeFrom').val().trim() === "") {
        $('#MadeFrom').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#MadeFrom').css('border-color', 'lightgrey');
    }

    if ($('#Quantity').val().trim() === "") {
        $('#Quantity').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Quantity').css('border-color', 'lightgrey');
    }

    if ($('#Value').val().trim() === "") {
        $('#Value').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Value').css('border-color', 'lightgrey');
    }
    if ($('#Tag').val().trim() === "") {
        $('#Tag').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Tag').css('border-color', 'lightgrey');
    }

    if ($('#Image1').val().trim() === "") {
        $('#Image1').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Image1').css('border-color', 'lightgrey');
    }

    if ($('#Seo').val().trim() === "") {
        $('#Seo').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Seo').css('border-color', 'lightgrey');
    }
    return isvalidate;
}


// Script cho phan Add: them moi Content
function Add() {
    //alert(CKEDITOR.instances['Remarks'].getData());  
    var res = validate();
    if (res == false) {
        return false;
    }
    var image = $('#Image1').val() + "|" + $('#Image2').val() + "|" + $('#Image3').val() + "|" + $('#Image4').val();
    var product =
    {
        Id: $('#Id').val(),
        Name: $('#Name').val(),
        MadeFrom: $('#MadeFrom').val(),
        CategoryId: $('#CategoryId').val(),
        Quantity: $('#Quantity').val(),
        Value: $('#Value').val(),
        Tag: $('#Tag').val(),
        //Image: $('#Image').val(),
        Image: image,
        Seo: $('#Seo').val(),
        Remark: CKEDITOR.instances['Remark'].getData(),
        Status: 1
    };
    //alert(JSON.stringify(category));
    $.ajax({
        url: "/Base/InsertProduct",
        data: JSON.stringify(product),
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
    var image = $('#Image1').val() + "|" + $('#Image2').val() + "|" + $('#Image3').val() + "|" + $('#Image4').val();
    var product =
    {
        Id: $('#Id').val(),
        Name: $('#Name').val(),
        MadeFrom: $('#MadeFrom').val(),
        CategoryId: $('#CategoryId').val(),
        Quantity: $('#Quantity').val(),
        Vallue: $('#Value').val(),
        Tag: $('#Tag').val(),
        //Image: $('#Image').val(),
        Image: image,
        Seo: $('#Seo').val(),
        Remark: CKEDITOR.instances['Remark'].getData(),
        Status: 1
    };

    $.ajax({
        url: "/Base/UpdateProduct",
        data: JSON.stringify(product),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function (result) {
            bootbox.alert('Update thành công!');
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
    $('#Id').val("");
    $('#Name').val("");
    $('#MadeFrom').val("");
    $('#CategoryId').find('option').remove().end();
    $('#Quantity').val("");
    $('#Value').val("");
    $('#Tag').val("");
    $('#Image1').val("");
    $('#Image2').val("");
    $('#Image3').val("");
    $('#Image4').val("");
    $('#Seo').val("");
    //$('#Remarks').val("");
    CKEDITOR.instances['Remark'].setData("")

    $('#Id').removeAttr('disabled');
    $('#Name').removeAttr('disabled');
    $('#MadeFrom').removeAttr('disabled');
    $('#CategoryId').removeAttr('disabled');
    $('#Quantity').removeAttr('disabled');
    $('#Value').removeAttr('disabled');
    $('#Tag').removeAttr('disabled');
    $('#Image1').removeAttr('disabled');
    $('#Image2').removeAttr('disabled');
    $('#Image3').removeAttr('disabled');
    $('#Image4').removeAttr('disabled');
    $('#Seo').removeAttr('disabled');
    $('#Remark').removeAttr('disabled');

    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Name').css('border-color', 'lightgrey');
    $('#CategoryId').css('border-color', 'lightgrey');
    $('#Quantity').css('border-color', 'lightgrey');
    $('#Value').css('border-color', 'lightgrey');
    $('#Seo').css('border-color', 'lightgrey');
    $('#Remark').css('border-color', 'lightgrey');


}
// mở popup khi bấm nút add category
function addpopup() {
    $('#myModalLabel').html('<h4><span class="glyphicon glyphicon-envelope"></span> Thêm mới xe</h4>');
    $('#myModal').modal('show');
    clearTextBox();
    loadcategoryid();
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/Base/GetNextProductId",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result);
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
                    url: "/Base/DeleteProduct/" + ID,
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

