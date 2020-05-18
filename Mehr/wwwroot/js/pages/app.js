﻿
// delete Colleague
function deleteColleague(id) {
    $.get("/Colleague/Delete/" + id, function (result) {
        $(".modal").modal('show');
        $(".modal-title").html("Delete Colleague");
        $(".modal-body").html(result);
    });
};

// search Colleague in List
function ColleagueSearch() {
    var value = $("#input-search").val().toLowerCase();
    $("#recent-buyers *").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
};

// Search Transactions By Sponsor Name
function SeaechBySponsorName() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("SponsorSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("MyTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByClassName("sponsor")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
};

function btnImportClick() {
    var btnImportForm = document.getElementById("btnImportForm");
    btnImportForm.click();
};

function changefileExcel() {
    var input = document.getElementById("btnImportForm");
    if (input.files && input.files[0]) {
        var ImportForm = document.getElementById("ImportForm");
        ImportForm.submit();
    }
};

// Upload Image
function changeImg(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#ImgUpload')
                .attr('src', e.target.result)
        };

        reader.readAsDataURL(input.files[0]);
    }
    else {
        var checker = document.getElementById("Gender");
        document.getElementById("ImgUpload").src = checker.checked ? "/images/Profiles/Male.jpg" : "/images/Profiles/Female.jpg";
    }
};

function imgUpClick() {
    var fileupload = document.getElementById("FileUpload");
    fileupload.click();
}

function fileUpChange(file) {
    changeImg(file);
}

// Gender
function changeGender() {
    var checker = document.getElementById("Gender");
    document.getElementById("hideIsMale").value = checker.checked ? "true" : "false";

    var fileupload = document.getElementById("FileUpload");
    if (!fileupload.files || !fileupload.files[0]) {
        document.getElementById("ImgUpload").src = checker.checked ? "/images/Profiles/Male.jpg" : "/images/Profiles/Female.jpg";
    }
}

// Date Picker
//

function setSumAmount(value) {
    $("#sumAmount").html(value);
}

function autoScale() {
    var x = document.getElementById("amount0");
    var y = document.getElementById("amount1");
    x.style.width = ((x.value.length + 2) * 8) + 'px';
    y.style.width = ((y.value.length + 2) * 8) + 'px';
};

function autoScaleSlider() {
    var min = document.getElementById("amount0").value;
    var max = document.getElementById("amount1").value;
    createSlider(Number(min), Number(max));
    autoScale();
    SeaechByAmountRange();
};

function SeaechByAmountRange() {
    var val0, val1, table, tr, td, i, txtValue, sum;
    sum = 0;
    val0 = Number(document.getElementById("amount0").value);
    val1 = Number(document.getElementById("amount1").value);
    table = document.getElementById("MyTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByClassName("amount")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (Number(txtValue) <= val0 || Number(txtValue) >= val1) {
                tr[i].style.display = "none";
            } else {
                tr[i].style.display = "";
                sum = sum + parseFloat(txtValue);
            }
        }
    }
    setSumAmount(sum);
};