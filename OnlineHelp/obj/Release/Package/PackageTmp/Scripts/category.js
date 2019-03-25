var ref;
ref = $("#myTable").DataTable({ "scrollX": true });
$(document).ready(function () {
    loaddatabyHung();
    loaddata_mappingscreen();
    loaddropdownlevel();
});
//function load level cho dropdown level
function loaddropdownlevel() {
    var html = '<option value="0"> All Level </option>';
    var html1 = '<option disabled selected value> -- select an option -- </option>';
    $.ajax({
        url: "/Home/GetListLevel",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {

                html += '<option value="' + value + '">' + value + '</option>';
                html1 += '<option value="' + value + '">' + value + '</option>';
            });
            $("#levelfilter").html(html); // load tại filter lọc
            $("#Level").html(html1); //load cho trong modal
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loaddropdownlevel1(c) {
    var html = '<option value="0"> All Level </option>';
    var html1 = '<option disabled selected value> -- select an option -- </option>';
    $.ajax({
        url: "/Home/GetListLevel",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, value) {
                if (value != c)
                    html += '<option value="' + value + '">' + value + '</option>';
                else html += '<option value="' + value + '" selected>' + value + '</option>';
                html1 += '<option value="' + value + '">' + value + '</option>';
            });
            $("#levelfilter").html(html); // load tại filter lọc
            $("#Level").html(html1); //load cho trong modal
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
// function loaddata khi chon level
function loaddatabylevel(level) {
    ref.clear().draw();
    var html = '';
    $.ajax({
        url: "/Home/GetListCategoryByLevel/" + level,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                html = '<a href="#" onclick="createchild(' + item.CategoryID + ',' + item.Level + ');"> Create Child </a> |<a href="#" onclick="showcontent(' + item.CategoryID + ');"> Details </a> | <a href="#" onclick="return getbyID(' + item.CategoryID + ');"> Edit</a> | <a href="#" onclick="Delete(' + item.CategoryID + ');">Delete</a>';
                ref.row.add([
                    item.CategoryID,
                    item.Level,
                    item.CategoryName,
                    item.ParentCategoryID,
                    item.Description,
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

// function load drop down list cho parent category id
function loaddropdownparentcategory() {
    $.ajax({
        url: "/Home/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '<option value="0">No Parent Category </option>';
            $.each(result, function (key, item) {
                html += '<option value="' + item.CategoryID + '">' + item.CategoryName + '</option>';
            });
            $("#ParentCategoryId").html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

// Khi level được chọn sẽ tải những Category có level nhở hơn để cho chọn cha của nó
function Getparentcategorybylevelofchild(level1) {
    $('#ParentCategoryId').find('option').remove().end();
    $.ajax({
        url: "/Home/Getparentcategorybylevelofchild/" + level1,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (!$.trim(result)) {
                $('#ParentCategoryId').append('<option value="0">No Parent Code</option>');
            }
            else {
                $.each(result, function (key, item) {
                    $('#ParentCategoryId').append('<option value="' + item.CategoryID + '">' + item.CategoryName + '</option>');
                });
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetNewlevel() {
    $.ajax({
        url: "/Home/GetNextLevel",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#Level').append('<option value="' + result + '">' + result + '</option>');
            $('#Level').val(result);
            $('#btnCreatenewlevel').prop('disabled', true);
            Getparentcategorybylevelofchild(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
// load data by Hung
function loaddatabyHung() {
    ref.clear().draw();
    var html = '';
    $.ajax({
        url: "/Home/List",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                html = '<a href="#" onclick="createchild(' + item.CategoryID + ',' + item.Level + ');"> Create Child </a> |<a href="#" onclick="showcontent(' + item.CategoryID + ');"> Details </a> | <a href="#" onclick="return getbyID(' + item.CategoryID + ');"> Edit</a> | <a href="#" onclick="Delete(' + item.CategoryID + ');">Delete</a>';
                ref.row.add([

                    item.CategoryID,
                    item.Level,
                    item.CategoryName,
                    item.ParentCategoryID,
                    item.Description,
                    html
                ]);
            });
            ref.draw(false);
            $("#loading").hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText); GetNewlevel
        }
    });
}

// Script cho phan Add: them moi Content
function Add() {
    //alert(CKEDITOR.instances['Remarks'].getData());    
    var res = validate();
    if (res == false) {
        return false;
    }
    var category =
    {
        CategoryID: $('#CategoryID').val(),
        Level: $('#Level option:selected').val(),
        CategoryName: $('#CategoryName').val(),
        ParentCategoryID: $('#ParentCategoryId option:selected').val(),
        MappingScreen: $('#MappingScreen option:selected').val(),
        EditDate: "",
        Editor: "FPT",
        Description: $('#Description').val(),
        Remarks: CKEDITOR.instances['Remarks'].getData()
    };
    $.ajax({
        url: "/Home/Add",
        data: JSON.stringify(category),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function (result) {
            bootbox.alert('Add new category success!');
            loaddatabylevel($('#Level option:selected').val());
            loaddropdownlevel();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}
// tạo child cho category tại bảng, 
function createchild(categoryid, categorylevel) {
    var i = categorylevel + 1;
    $('#myModalLabel').html('<h4><span class="glyphicon glyphicon-envelope"></span> Add New Category</h4>');
    $('#myModal').modal('show');
    clearTextBox();
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/Home/GetNextCategoryID",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            // lấy categoryid
            $('#CategoryID').val(result);
            // lấy parent category id
            $('#ParentCategoryId').find('option').remove().end();
            $('#ParentCategoryId').append('<option value="' + categoryid + '">' + categoryid + '</option>');
            $("#ParentCategoryId").attr("disabled", "disabled");
            // lấy level nữa
            $('#Level').find('option').remove().end();
            $('#Level').append('<option value="' + i + '">' + i + '</option>');
            $("#Level").attr("disabled", "disabled");
            // disable button theem moi level
            $("#btnCreatenewlevel").attr("disabled", "disabled");
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
    var category =
    {
        CategoryID: $('#CategoryID').val(),
        Level: $('#Level option:selected').val(),
        CategoryName: $('#CategoryName').val(),
        ParentCategoryID: $('#ParentCategoryId option:selected').val(),
        MappingScreen: $('#MappingScreen option:selected').val(),
        EditDate: "",
        Editor: "FPT",
        Description: $('#Description').val(),
        Remarks: CKEDITOR.instances['Remarks'].getData()
    };
    $.ajax({
        url: "/Home/Update",
        data: JSON.stringify(category),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function (result) {
            //loaddatabyHung();
            var c = $('#Level option:selected').val();
            loaddatabylevel(c);
            //loaddropdownlevel();
            loaddropdownlevel1(c);
            $('#myModal').modal('hide');

            bootbox.alert('Update success !');
        },
        error: function (errormessage) {
            bootbox.alert(errormessage.responseText);
        }
    });

}
// function delete ID
function Delete(ID) {
    bootbox.confirm({
        title: "Delete Category?",
        message: "Do you want to delete this category?",
        buttons: {
            cancel: {
                label: '<i class="glyphicon glyphicon-remove"></i> Cancel'
            },
            confirm: {
                label: '<i class="glyphicon glyphicon-ok"></i> Confirm'
            }
        },
        callback: function (a) {
            if (a) {
                $.ajax({
                    url: "/Home/Delete/" + ID,
                    type: "POST",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        if (result == -1) {
                            bootbox.alert("You cannot this category. You must delete all child of it");

                        }
                        else {
                            //loaddatabyHung();   
                            var b = $('#levelfilter option:selected').val();
                            // load lại tại đúng level đó
                            loaddatabylevel(b);
                            // load lại level ở trang
                            loaddropdownlevel();
                            bootbox.alert("Delete Successful!");
                        }
                        //loaddropdownparentcatgory();
                    },
                    error: function (errormessage) {
                        alert(errormessage.responseText);
                    }
                });
            }
        }
    });

}

// function lấy dữ liệu theo category ID để chỉnh sửa
function getbyID(ID) {
    $('#CategoryID').css('border-color', 'lightgrey');
    $('#Level').css('border-color', 'lightgrey');
    $('#Level').attr('disabled', 'disabled');
    $('#CategoryName').css('border-color', 'lightgrey');
    $('#ParentCategoryId').css('border-color', 'lightgrey');
    $('#ParentCategoryId').attr('disabled', 'disabled');
    $('#Description').css('border-color', 'lightgrey');
    $('#Remarks').css('border-color', 'lightgrey');
    $('#btnCreatenewlevel').attr('disabled', 'disabled');

    $.ajax({
        url: "/Home/GetByID/" + ID,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#CategoryID').val(result.CategoryID);
            $('#Level').find('option').remove().end();
            $('#Level').append('<option value="' + result.Level + '">' + result.Level + '</option>');
            $('#CategoryName').val(result.CategoryName);
            $('#ParentCategoryId').find('option').remove().end();
            $('#ParentCategoryId').append('<option value="' + result.ParentCategoryID + '">' + result.ParentCategoryID + '</option>');
            $('#Description').val(result.Description);
            $('#MappingScreen').find('option').remove().end();
            loaddropdown_mappingscreen1(result.MappingScreen);
            //$('#MappingScreen option[value=re]').val(result.MappingScreen);
            //var html1 = '<option selected value>' + result.MappingScreen + '</option>';
            //$('#MappingScreen').append(html1);
            //$("#MappingScreen option[value='"+result.MappingScreen+"']").prop('selected', true);
            //$('#MappingScreen')
            CKEDITOR.instances['Remarks'].setData(result.Remarks);

            $('#myModalLabel').html('<span class="glyphicon glyphicon-envelope"></span> Edit Category');
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
    if ($('#Level option:selected').val().trim() === "") {
        $('#Level').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Level').css('border-color', 'lightgrey');
    }

    if ($('#CategoryName').val().trim() === "") {
        $('#CategoryName').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#CategoryName').css('border-color', 'lightgrey');
    }

    if ($('#ParentCategoryId option:selected').val().trim() === "") {
        $('#ParentCategoryId').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#ParentCategoryId').css('border-color', 'lightgrey');
    }

    if ($('#Description').val().trim() === "") {
        $('#Description').css('border-color', 'red');
        isvalidate = false;
    }
    else {
        $('#Description').css('border-color', 'lightgrey');
    }
    return isvalidate;
}
// function clear text box trong modal
function clearTextBox() {
    $('#CategoryID').val("");
    $('#CategoryName').val("");
    $('#Level').val("");
    //$('#ParentCategoryId').val("");
    $('#ParentCategoryId').find('option').remove().end();
    $('#Description').val("");
    //$('#Remarks').val("");
    CKEDITOR.instances['Remarks'].setData("")

    $('#CategoryName').removeAttr('disabled');
    $('#Level').removeAttr('disabled');
    $('#ParentCategoryId').removeAttr('disabled');
    $('#Description').removeAttr('disabled');
    $('#Remarks').removeAttr('disabled');
    $('#btnCreatenewlevel').removeAttr('disabled');

    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#CategoryName').css('border-color', 'lightgrey');
    $('#ParentCategoryID').css('border-color', 'lightgrey');
    $('#Description').css('border-color', 'lightgrey');
    $('#Remarks').css('border-color', 'lightgrey');

}
// mở popup khi bấm nút add category
function addpopup() {
    $('#myModalLabel').html('<h4><span class="glyphicon glyphicon-envelope"></span> Add New Category</h4>');
    $('#myModal').modal('show');
    clearTextBox();
    loaddropdownlevel();
    loaddropdown_mappingscreen();
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $.ajax({
        url: "/Home/GetNextCategoryID",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#CategoryID').val(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
// Show content popup hiển thị chi tiết nội dung category đó
function showcontent(categoryid) {
    $.ajax({
        url: "/Home/GetByID/" + categoryid,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Remarks) {
                $('#modalshowcontent_body').html(result.Remarks);
            }
            else {
                $('#modalshowcontent_body').html('No Content In Remarks');
            }
            $('#modalshowcontent').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function importexcel() {
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;
    /*Checks whether the file is a valid excel file*/
    if (regex.test($("#excelfile").val().toLowerCase())) {
        var xlsxflag = false; /*Flag for checking whether excel is .xls format or .xlsx format*/
        if ($("#excelfile").val().toLowerCase().indexOf(".xlsx") > 0) {
            xlsxflag = true;
        }
        /*Checks whether the browser supports HTML5*/
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                /*Converts the excel data in to object*/
                if (xlsxflag) {
                    var workbook = XLSX.read(data, { type: 'binary' });
                }
                else {
                    var workbook = XLS.read(data, { type: 'binary' });
                }
                /*Gets all the sheetnames of excel in to a variable*/
                var sheet_name_list = workbook.SheetNames;

                var cnt = 0; /*This is used for restricting the script to consider only first sheet of excel*/
                sheet_name_list.forEach(function (y) { /*Iterate through all sheets*/
                    /*Convert the cell value to Json*/
                    //if (xlsxflag) {
                    //var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);
                    // }
                    // else {
                    var exceljson = XLS.utils.sheet_to_row_object_array(workbook.Sheets[y]);
                    // }
                    update_mappingscreen(exceljson);
                    if (exceljson.length > 0 && cnt == 0) {
                        BindTable(exceljson, '#myTable2');
                        cnt++;
                    }
                });
                $('#myTable2').show();
            }
            if (xlsxflag) {/*If excel file is .xlsx extension than creates a Array Buffer from excel*/
                reader.readAsArrayBuffer($("#excelfile")[0].files[0]);
            }
            else {
                reader.readAsBinaryString($("#excelfile")[0].files[0]);
            }
        }
        else {
            alert("Sorry! Your browser does not support HTML5!");
        }
    }
    else {
        alert("Please upload a valid Excel file!");
    }
}
function exportexcel() {
    $('#myTable2').table2excel({
        name: "Table2Excel",
        filename: "myFileName",
        fileext: ".xls"
    });
}
function BindTable(jsondata, tableid) {/*Function used to convert the JSON array to Html Table*/
    $(tableid).empty();
    var columns = BindTableHeader(jsondata, tableid); /*Gets all the column headings of Excel*/
    for (var i = 0; i < jsondata.length; i++) {
        var row$ = $('<tr/>');
        for (var colIndex = 0; colIndex < columns.length; colIndex++) {
            var cellValue = jsondata[i][columns[colIndex]];
            if (cellValue == null)
                cellValue = "";

            row$.append($('<td/>').html(cellValue));
        }
        $(tableid).append(row$);
    }
}
function BindTableHeader(jsondata, tableid) {/*Function used to get all column names from JSON and bind the html table header*/
    var columnSet = [];
    var headerTr$ = $('<tr/>');
    for (var i = 0; i < jsondata.length; i++) {
        var rowHash = jsondata[i];
        for (var key in rowHash) {
            if (rowHash.hasOwnProperty(key)) {
                if ($.inArray(key, columnSet) == -1) {/*Adding each unique column names to a variable array*/
                    columnSet.push(key);
                    headerTr$.append($('<th/>').html(key));
                }
            }
        }
    }
    $(tableid).append(headerTr$);
    return columnSet;
}

// function load drop down list cho mapping screen
function loaddropdown_mappingscreen() {
    $.ajax({
        url: "/Home/GetMappingSCreen",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<option value="' + item.ScreenID + '">' + item.ScreenName + '</option>';
            });
            $("#MappingScreen").html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function update_mappingscreen(screenmapping) {
    var postData = { screenmapping1: JSON.stringify(screenmapping) }
    $.ajax({
        url: "/Home/UpdateMappingScreen",
        data: postData,
        type: "POST",
        contenType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            bootbox.alert("Cập nhật mapping screen thành công !");
        },
        error: function (errormessage) {
            bootbox.alert(errormessage.responseText);
        }
    });
}
function loaddata_mappingscreen() {
    $.ajax({
        url: "/Home/GetMappingSCreen",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#myTable2').append("<tr><td>ScreenId</td><td>ScreenName</td></tr>");
            //$('#myTable2').append("<tr><td>gfdgdg</td><td>gfdgdf</td></tr>");
            $.each(result, function (key, item) {
                //$('#myTable2').append("<tr>");
                //$('#myTable2').append("<td>" + item.ScreenID + "</td>");
                //$('#myTable2').append("<td>" + item.ScreenName + "</td>");
                //$('#myTable2').append("</tr>");
                $('#myTable2').append("<tr><td>" + item.ScreenID + "</td><td>" + item.ScreenName + "</td></tr>");
            });
            //$('#myTable2').append("</tbody>")
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function loaddropdown_mappingscreen1(_mappingscreen) {
    $.ajax({
        url: "/Home/GetMappingSCreen",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                if (item.ScreenID != _mappingscreen)
                    html += '<option value="' + item.ScreenID + '">' + item.ScreenName + '</option>';
                else
                    html += '<option value="' + item.ScreenID + '" selected>' + item.ScreenName + '</option>';
            });
            $("#MappingScreen").html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}