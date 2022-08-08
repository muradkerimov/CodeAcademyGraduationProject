$(document).ready(function () {
    $('.owl-carousel').owlCarousel({
        loop: true,
        nav: false,
        responsive: {
            0: {
                items: 1
            },
            1000: {
                items: 1
            }
        },
        dots: false
    }
    )



    let skip = 3;
    let concertsCount = $("#concertsCount").val();
    $(document).on("click", "#load-more-button", function (e) {
        e.preventDefault();
        $.ajax({
            url: "/Concerts/LoadMoreIndex?skip=" + skip,
            type: "get",
            success: function (res) {
                skip += 3;
                $(".concert-wrapper").append(res);
                if (skip >= concertsCount) {
                    $("#load-more-button").remove();
                }
            }
        })
    })

    const inputs = document.querySelectorAll('input');

    inputs.forEach(el => {
        el.addEventListener('blur', e => {
            if (e.target.value) {
                e.target.classList.add('dirty');
            } else {
                e.target.classList.remove('dirty');
            }
        })
    })

    const hiddenInputId = document.querySelector("#hidenInputId");
    const hiddenInputName = document.querySelector("#hidenInputName");
    const hiddenInputTime = document.querySelector("#hidenInputTime");
    const seatsOfRow = document.querySelectorAll('.row .seat');
    if (hiddenInputId !== null && hiddenInputName !== null && hiddenInputTime !== null && seatsOfRow !== null) {

        let getHiddenInputIdVal = hiddenInputId.value;
        let gethiddenInputNameVal = hiddenInputName.value;
        let gethiddenInputTimeVal = hiddenInputTime.value;

        let finalizeArray = JSON.parse(localStorage.getItem('busket')) !== null ? JSON.parse(localStorage.getItem('busket')) : [];

        if (finalizeArray.length > 0) {
            for (let i = 0; i < finalizeArray.length; i++) {
                if (getHiddenInputIdVal === finalizeArray[i].concertId) {
                    for (let j = 0; j < seatsOfRow.length; j++) {
                        let dataSetId = seatsOfRow[j].getAttribute("data-seatId");
                        if (finalizeArray[i].seatId === dataSetId) {
                            seatsOfRow[j].classList.add("selected");
                            break;
                        }
                    }
                }
            }
        };

        for (let i = 0; i < seatsOfRow.length; i++) {
            seatsOfRow[i].addEventListener('click', function () {
                if (
                    this.classList.contains("seat") &&
                    !this.classList.contains("sold")
                ) {
                    if (!this.classList.contains("selected")) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Səbətə əlavə olundu',
                        });
                        this.classList.add("selected");
                        let dataId = getHiddenInputIdVal + '' + this.getAttribute("data-seatId");

                        const prepareArray = [...finalizeArray, {
                            concertId: getHiddenInputIdVal,
                            seatId: this.getAttribute("data-seatId"),
                            ticketIdent: dataId,
                            concertName: gethiddenInputNameVal,
                            concertDate: gethiddenInputTimeVal,
                        }];

                        finalizeArray = prepareArray;
                        localStorage.setItem("busket", JSON.stringify(finalizeArray));
                        this.setAttribute("data-id", dataId);
                    } else {
                        Swal.fire({
                            icon: 'success',
                            title: 'Səbətdən cıxarıldı',
                        });
                        this.classList.remove("selected");
                        this.removeAttribute("data-id");
                        let dataSeatId = this.getAttribute("data-seatId");
                        const removeSeat = finalizeArray.filter((item) => item.seatId !== dataSeatId);
                        finalizeArray = removeSeat;
                        localStorage.setItem("busket", JSON.stringify(removeSeat));
                    }
                }
            })
        };
    }

    let datas = JSON.parse(localStorage.getItem('busket')) !== null ? JSON.parse(localStorage.getItem('busket')) : [];
    $("#buytickets").attr("href", "/concerts/buytickets?ticketdata=" + localStorage.getItem('busket'))
    let busketWrapper = document.querySelector("#busket-wrapper");


    if (datas.length > 0) {
        for (let i = 0; i < datas.length; i++) {

            let totalSeatNumberEachRow = 8;
            let rowNumber = Math.ceil(datas[i].seatId / totalSeatNumberEachRow);
            let seatNumber = (datas[i].seatId % totalSeatNumberEachRow) !== 0 ? datas[i].seatId % totalSeatNumberEachRow : totalSeatNumberEachRow;

            let col12 = document.createElement("div");
            let cardBox = document.createElement("div");
            let box = document.createElement("box");
            let boxUp = document.createElement("div");
            let boxMiddle = document.createElement("div");
            let boxDown = document.createElement("div");
            let seatRowNumber = document.createElement("p");
            let seatColumnNumber = document.createElement("p");
            let concertName = document.createElement("p");
            let concertDate = document.createElement("p");
            let viewConcert = document.createElement("a");
            let deleteConcert = document.createElement("a");

            col12.classList.add("col-md-12");
            col12.classList.add("mt-2");
            col12.classList.add("mb-2");
            cardBox.classList.add("card-box");
            box.classList.add("box");
            boxUp.classList.add("box-up");
            boxMiddle.classList.add("box-middle");
            boxDown.classList.add("box-down");
            concertName.classList.add("distance-left");
            seatColumnNumber.classList.add("distance-left");
            deleteConcert.classList.add("distance-left");

            deleteConcert.setAttribute("href", "#");
            deleteConcert.setAttribute("data-id", datas[i].ticketIdent);
            viewConcert.setAttribute("href", "/concerts/info/" + datas[i].concertId);


            seatRowNumber.innerHTML = `<strong>Row Number: </strong> ${rowNumber}`;
            seatColumnNumber.innerHTML = `<strong>Seat Number: </strong> ${seatNumber}`;
            concertName.innerHTML = `<strong>Concert Name: </strong> ${datas[i].concertName}`;
            concertDate.innerHTML = `<strong>Concert Date: </strong> ${datas[i].concertDate}`;
            deleteConcert.innerText = "Remove busket";
            viewConcert.innerText = "View concert";

            busketWrapper.append(col12);
            col12.append(cardBox);
            cardBox.append(box);
            box.append(boxUp);
            box.append(boxMiddle);
            box.append(boxDown);
            boxUp.append(seatRowNumber);
            boxUp.append(seatColumnNumber);
            boxMiddle.append(concertName);
            boxMiddle.append(concertDate);
            boxDown.append(deleteConcert);
            boxDown.append(viewConcert);

            deleteConcert.addEventListener("click", function () {
                let getTicketId = this.getAttribute("data-id");
                let newBucketData = datas.filter((item) => item.ticketIdent !== getTicketId);
                localStorage.setItem("busket", JSON.stringify(newBucketData));
                window.location.reload()
            })

        }
    } else {
        let col12 = document.createElement("div");
        let notFoundText = document.createElement("p");
        let buytickets = document.querySelector("#buytickets");
        buytickets.classList.add("d-none")
        col12.classList.add("col-md-12");
        notFoundText.classList.add("not_found_text");
        notFoundText.innerText = "Not found data in your basket";
        col12.append(notFoundText);
        busketWrapper.append(col12);
    }
});