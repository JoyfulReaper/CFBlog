let index = 0;

function AddTag() {
    var tagEntry = document.getElementById("TagEntry");

    let validationResult = ValidateTags(tagEntry.value);
    if (validationResult != null) {
        swalWithDarkButton.fire({
            html: `<span class='font-weigh-bolder'>${validationResult.toUpperCase()}</span>`
        });

        return;
    }

    let newOption = new Option(tagEntry.value, tagEntry.value);
    document.getElementById("TagValues").options[index++] = newOption;

    tagEntry.value = "";
    return true;
}

function DeleteTag() {
    let tagCount = 1;

    let tagList = document.getElementById("TagValues");
    if (!tagList) {
        return false;
    }

    if (tagList.selectedIndex == -1) {
        swalWithDarkButton.fire({
            html: `<span class='font-weight-bolder''>CHOOSE A TAG BEFORE DELETING</span>`
        });
        return true;
    }

    while (tagCount > 0) {
        if (tagList.selectedIndex >= 0) {
            tagList.options[tagList.selectedIndex] = null;
            --tagCount;
        }
        else
            tagCount = 0;
        index--;
    }
}

$("form").on("submit", function () {
    $("#TagValues option").prop("selected", "selected");
})

if (tagValues != '') {
    let tagArray = tagValues.split(",");
    for (let i = 0; i < tagArray.length; i++) {
        ReplaceTag(tagArray[i], i);
        index++;
    }
}

function ReplaceTag(tag, index) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagValues").options[index] = newOption;
}

// Detect empty or duplicate tag
function ValidateTags(str) {
    if (str == "") {
        return 'Empty Tags are not permitted'
    }

    var tagsElement = document.getElementById('TagValues');
    if (tagsElement) {
        let options = tagsElement.options;
        for (let i = 0; i < options.length; i++) {
            if (options[i].value == str) {
                return `The Tag #${str} was detected as a duplicate tag and has not been added a second time.`;
            }
        }
    }
}

const swalWithDarkButton = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-danger btn-sm w-75 btn-outline-dark'
    },
    imageUrl: '/img/error.jpg',
    timer: 3000,
    buttonsStyling: false
});