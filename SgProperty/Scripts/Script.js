var count = 0; //want it to be global variable
var checkingone = "";
var checkingtwo = "";

var property1Taken = false;     // Flag indicating hidden input #compare1 used or unused. True: used, False: not used
var property2Taken = false;     // Flag indicating hidden input #compare2 used or unused. True: used, False: not used

var ajaxCompareURL = '/Property/SetSessionCompareAjax';

$(document).ready(function () {

    // Fill hidden inputs with retrieved session variables
    $('#compare1').val(compare1)    // var compare1 initialized in Listing.cshtml with Session["compare1"]
    $('#compare2').val(compare2)    // var compare2 initialized in Listing.cshtml with Session["compare2"]
    
    // Set flags accordingly
    if (compare1) { // compare1 will evaluate to false if it is: null, undefined, empty string (""), 0, NaN
        property1Taken = true;
        checkingone = compare1;
        count += 1;
        console.log("property1Taken = true");
    }
    if (compare2) { // compare2 will evaluate to false if it is: null, undefined, empty string (""), 0, NaN
        property2Taken = true;
        checkingtwo = compare2;
        count += 1;
        console.log("property2Taken = true");
    }

    if (property1Taken && property2Taken) {
        document.getElementById('submitbtn').disabled = false;  // Enable Compare button
    }

    //TODO: Set checkbox ticked for selected compares if compare exists in page
    var propertyCheckboxesInPage = document.getElementsByName("checkboxCompare");
    console.log("getElementsByName for checkbox");
    console.log(propertyCheckboxesInPage);
    for (var i = 0; i < propertyCheckboxesInPage.length; i++) {
        var checkbox = propertyCheckboxesInPage[i];
        if (checkbox.id && (checkbox.id == checkingone || checkbox.id == checkingtwo)) {   // Ignore hidden checkbox filled in by Razor
            console.log(checkbox.id);
            checkbox.checked = true;
        }
    }

    //TODO: Change from property id to property name


    // Checkbox onclick listener
    $('input[type="checkbox"]').click(function () {

        if ($(this).is(":checked")) {
            //if checkbox is checked
            console.log(this.value);
            if (count == 0) {
                if (property1Taken == false && property2Taken == false) {
                    $('#compare1').val(this.value)  //assign value to hidden attribute
                    count = count + 1;
                    checkingone = this.value;

                    // Set session variables through ajax
                    ajaxCall(ajaxCompareURL, checkingone, checkingtwo);

                    property1Taken = true;
                    $('#property1').html(checkingone);
                    console.log(document.getElementById('compare1').value);

                }
            }
            else if (count == 1) {
                if (property1Taken == true && property2Taken == false) {
                    $('#compare2').val(this.value)
                    count = count + 1;
                    checkingtwo = this.value;

                    // Set session variables through ajax
                    ajaxCall(ajaxCompareURL, checkingone, checkingtwo);

                    property2Taken = true;
                    $('#property2').html(checkingtwo);
                    document.getElementById('submitbtn').disabled = false;

                }
                else if (property1Taken == false && property2Taken == true) {
                    $('#compare1').val(this.value)  //assign value to hidden attribute
                    count = count + 1;
                    checkingone = this.value;

                    // Set session variables through ajax
                    ajaxCall(ajaxCompareURL, checkingone, checkingtwo);

                    property1Taken = true;
                    $('#property1').html(checkingone);
                    document.getElementById('submitbtn').disabled = false;

                    console.log(document.getElementById('compare1').value);

                }

            }
            else if (count == 2) {
                alert("Can only select two checkbox");
                document.getElementById(this.value).checked = false;

            }
        }
        else if ($(this).is(":not(:checked)")) {
            //if checkbox is unchecked
            count = count - 1;
            document.getElementById('submitbtn').disabled = true;
            // determine which value to remove
            if (this.value == checkingone) {
                $('#compare1').val('')  // reset hidden1
                property1Taken = false;
                checkingone = "";   // reset checkingone

                // Set session variables through ajax
                ajaxCall(ajaxCompareURL, checkingone, checkingtwo);

                $('#property1').html(checkingone);
            }
            else if (this.value == checkingtwo) {
                $('#compare2').val('')
                property2Taken = false;
                checkingtwo = "";   // reset checkingtwo

                // Set session variables through ajax
                ajaxCall(ajaxCompareURL, checkingone, checkingtwo);

                $('#property2').html(checkingtwo);
            }
        }
    });

    function ajaxCall(url, compare1, compare2) {
        $.ajax({
            type: "POST",
            url: url,
            data: '{"compare1" : "' + compare1 + '", "compare2" : "' + compare2 + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
        console.log("ajaxCall");
    }
});