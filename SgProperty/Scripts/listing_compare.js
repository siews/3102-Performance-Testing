// Assume var compares initialized in view as array
// Assume var minCompare, maxCompare initialized in view as int holding minimum and maximum comparison counts

$document.ready(function () {

    // Function to perform ajax call to controller to set comparison session variables
    function ajaxSetSessionCompare(compare1, compare2) {
        $.ajax({
            type: "POST",
            url: '/Property/SetSessionCompareAjax',
            data: '{"compare1" : "' + compare1 + '", "compare2" : "' + compare2 + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
    }

    // Fill hidden inputs with retrieved session variables
    $('#compare1').val(compares[0]);
    $('#compare2').val(compares[1]);

    var propertyTaken = [];
    // Initialize propertyTaken flags
    for (i = 0; i < compares.length; i++) {
        if (compares[i]) {  // compares[i] will evaluate to true if NOT: null, undefined, empty string (""), 0, NaN
            propertyTaken.push(true);
        }
        else {
            propertyTaken.push(false);
        }
    }


    // Checkbox onclick listener
    if ($(this).is(":checked")) {
        // If maximum supported comparison values exceeded
        if (propertyTaken.allValuesTrue) {
            alert("Can only select" + compares.length + "checkbox");
            document.getElementById(this.value).checked = false;
        }
        else {
            // Some properties are taken

        }
    }
});

function getFirstEmptyStringOrNullOrFalseIndexInArray(array) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] == "" || array[i] == null || array[i] == false)
            return i;   // return array index where empty or null
    }
    return null;    // return null when no empty/null/false values found
}

Array.prototype.allValuesTrue = function () {
    for (var i = 0; i < this.length; i++) {
        if (this[i] !== true)
            return false;
    }
    return true;
}