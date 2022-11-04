const technical = document.getElementsByClassName("technical")
const technicalValue = document.getElementsByClassName("technicalValue")

for (let i = 0; i < technical.length; i++) {
    if (technicalValue[i].innerText == "Skilled") {
        technical[i].style.backgroundColor = "rgb(29 199 106)"
    } else if (technicalValue[i].innerText == "PartiallySkilled") {
        technical[i].style.backgroundColor = "rgb(253 197 61)"
        technicalValue[i].innerText = "Partially Skilled"
    } else if (technicalValue[i].innerText == "LowSkilled") {
        technical[i].style.backgroundColor = "rgb(255 120 20)"
        technicalValue[i].innerText = "Low Skilled"
    } else if (technicalValue[i].innerText == "Unskilled") {
        technical[i].style.backgroundColor = "rgb(235 74 62)"
    }
}

const consultant = document.getElementsByClassName("consultant")
const consulantValue = document.getElementsByClassName("consultantValue")

for (let i = 0; i < technical.length; i++) {
    if (consulantValue[i].innerText == "Skilled") {
        consultant[i].style.backgroundColor = "rgb(29 199 106)"
    } else if (consulantValue[i].innerText == "PartiallySkilled") {
        consultant[i].style.backgroundColor = "rgb(253 197 61)"
        consulantValue[i].innerText = "Partially Skilled"
    } else if (consulantValue[i].innerText == "LowSkilled") {
        consultant[i].style.backgroundColor = "rgb(255 120 20)"
        consulantValue[i].innerText = "Low Skilled"
    } else if (consulantValue[i].innerText == "Unskilled") {
        consultant[i].style.backgroundColor = "rgb(235 74 62)"
    }
}

