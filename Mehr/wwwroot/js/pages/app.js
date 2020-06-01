
// delete Colleague
function deleteColleague(id) {
    $.get("/Colleague/Delete/" + id, function (result) {
        $(".modal").modal('show');
        $(".modal-title").html("Delete Colleague");
        $(".modal-body").html(result);
    });
};

// delete Bank
function deleteBank(id) {
    $.get("/Bank/Delete/" + id, function (result) {
        $(".modal").modal('show');
        $(".modal-title").html("Delete Bank");
        $(".modal-body").html(result);
    });
};

// edit Bank
function editBank(id) {
    $.get("/Bank/Edit/" + id, function (result) {
        $(".modal").modal('show');
        $(".modal-title").html("Edit Bank");
        $(".modal-body").html(result);
    });
};

// new bank
function newBank() {
    $.get("/Bank/Create/", function (result) {
        $(".modal").modal('show');
        $(".modal-title").html("New bank");
        $(".modal-body").html(result);
    });
};

// new Bank data
function newBankData(id) {
    $.get("/BankData/Create/" + id, function (result) {
        $(".modal").modal('show');
        $(".modal-title").html("New Bank Transaction");
        $(".modal-body").html(result);
    });
}

// search Colleague in List
function ColleagueSearch() {
    var value = $("#input-search").val().toLowerCase();
    var items = $('.colleagueItem');
    items.each((h) => {
        console.log(items[h]);
        var isShow = items[h].innerText.toLowerCase().indexOf(value) > -1
        if (isShow) {
            items[h].style.display = "";
        } else {
            items[h].style.display = "none";
        }
    }
    );
   
};

// Search Transactions By Sponsor Name
function SeaechBySponsorName() {
    SeaechInTable("SponsorSearch", "sponsor")
};

// Search Transactions By Tracking Number
function SeaechByTrackingNumber() {
    SeaechInTable("TrackingNumber", "track")
};

// Search Transactions By Card Number
function SeaechByCardNumber() {
    SeaechInTable("LastFourNumbersOfBankCard", "cardNumber")
};

// Search Transactions in Table
function SeaechInTable(inputId, attrClass) {
    var input, filter, table, tr, td, i, txtValue, sum, tdAmount, amountValue;
    sum = 0;
    input = document.getElementById(inputId);
    filter = input.value.toUpperCase();
    table = document.getElementById("MyTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByClassName(attrClass)[0];
        if (td) {
            tdAmount = tr[i].getElementsByClassName("amount")[0];
            amountValue = tdAmount.textContent || tdAmount.innerText;
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
                sum = sum + parseFloat(amountValue);
            } else {
                tr[i].style.display = "none";
            }
        }
    }
    setSumAmount(sum);
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

// move focus
function moveFocus(from, to) {
    var length = from.value.length;
    var maxLength = from.getAttribute("maxLength");
    if (length == maxLength) {
        document.getElementById(to).focus();
    }
};  

function FillCardNumber() {
    var n1 = document.getElementById('CardNumber1').value;
    var n2 = document.getElementById('CardNumber2').value;
    var n3 = document.getElementById('CardNumber3').value;
    var n4 = document.getElementById('CardNumber4').value;

    document.getElementById("CardNumber").value = n1+n2+n3+n4;
} 

function FillShebaNumber() {
    var n1 = document.getElementById('ShebaNumber1').value;
    var n2 = document.getElementById('ShebaNumber2').value;
    var n3 = document.getElementById('ShebaNumber3').value;
    var n4 = document.getElementById('ShebaNumber4').value;
    var n5 = document.getElementById('ShebaNumber5').value;
    var n6 = document.getElementById('ShebaNumber6').value;
    var n7 = document.getElementById('ShebaNumber7').value;

    document.getElementById("ShebaNumber").value = 'IR' + n1 + n2 + n3 + n4 + n5 + n6 + n7;
}
