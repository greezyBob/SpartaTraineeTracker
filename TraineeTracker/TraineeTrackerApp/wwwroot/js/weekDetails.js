const technical = document.getElementById("technical")
const technicalValue = document.getElementById("technicalValue")

if (technicalValue.innerText == "Skilled") {
    technical.style.backgroundColor = "rgb(29 199 106)"
} else if (technicalValue.innerText == "PartiallySkilled") {
    technical.style.backgroundColor = "rgb(253 197 61)"
    technicalValue.innerText = "Partially Skilled"
} else if (technicalValue.innerText == "LowSkilled") {
    technical.style.backgroundColor = "rgb(255 120 20)"
    technicalValue.innerText = "Low Skilled"
} else if (technicalValue.innerText == "Unskilled") {
    technical.style.backgroundColor = "rgb(235 74 62)"
}

const consultant = document.getElementById("consultant")
const consulantValue = document.getElementById("consultantValue")
if (consulantValue.innerText == "Skilled") {
    consultant.style.backgroundColor = "rgb(29 199 106)"
} else if (consulantValue.innerText == "PartiallySkilled") {
    consultant.style.backgroundColor = "rgb(253 197 61)"
    consulantValue.innerText = "Partially Skilled"
} else if (consulantValue.innerText == "LowSkilled") {
    consultant.style.backgroundColor = "rgb(255 120 20)"
    consulantValue.innerText = "Low Skilled"
} else if (consulantValue.innerText == "Unskilled") {
    consultant.style.backgroundColor = "rgb(235 74 62)"
}