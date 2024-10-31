const host = `${window.location.protocol}//${window.location.host}`;

fetchApiRequest('GetAllEmployees');
fetchApiRequest('GetAllJobPostings');

async function fetchApiRequest(functionName) {
    try {
        var response = await fetch(host + '/' + functionName);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        console.log(response);

        var jsonResponse = await response.json();
        console.log('apiRequest function successful:', jsonResponse);
        switch (functionName) {
            case "GetAllJobPostings":
                displayJobs(jsonResponse);
                break;
        }

    } catch (error) {
        console.error('Failed to fetch employees:', error);
        alert('Failed to load employee data. Please check your connection.');
    }
}

function displayJobs(jobs) {
    const tbody = document.querySelector('#jobPostingsTableBody');

    console.log('Jobs data:', jobs);

    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    tbody.innerHTML = ''; // Clear any existing rows

    jobs.forEach(job => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td><a href="#container2"><button type="button" class="submit-button" onclick="AddNewApplicant('${job.postingId}')">Add Job</button></a></td>
            <td>${job.postingId}</td>
            <td>${job.Title}</td>
            <td>${job.Salary}</td>
            <td>${job.JobType}</td>
            <td>${job.Hours}</td>
            <td>${job.JobDescription}</td>
            <td>${job.JobDescription}</td>
        `;

        tbody.appendChild(row);
    });
}

async function AddNewApplicant(postingId) {
    const post = await FetchRequestGET(`GetJobPostByID/${postingId}`);
    console.log('Selected job post:', post);
}

function addNewJob() {
    var postingId = Math.random().toString(36).substring(2, 9); // Generate a random ID
    var jobTitle = document.getElementById("jobTitle").value;
    var jobDescription = document.getElementById("jobDescription").value;
    var jobType = document.getElementById("jobType").value;
    var salary = document.getElementById("jobSalary").value;
    var hours = document.getElementById("jobHours").value;
    var location = document.getElementById("jobLocation").value;

    // Validate required fields
    if (!jobTitle || !jobDescription || !jobType || !salary || !hours || !location) {
        alert("You have not answered all required fields");
        return;
    } else {

        var jobPosting = {
            "postingId": postingId,
            "Title": jobTitle,
            "Salary": salary,
            "JobDescription": jobDescription,
            "JobType": jobType,
            "Hours": hours,
            "LocationId": location
        };

        fetch('http://localhost:8000/InsertJobPosting', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(jobPosting)
        })
            .then(response => {
                if (response.ok) {
                    return response.text(); // Success message
                } else {
                    throw new Error('Error inserting the job posting');
                }
            })
            .then(data => {
                alert(data); // Show success message
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Failed to insert job postingawdwadwa');
            });
    }

    
}

function filterJobTable() {
    var input, filter, table, tr, td, i, txtValue;

    input = document.getElementById("jobTypes");
    filter = input.value.toUpperCase();
    table = document.getElementById("jobPostingsTable");
    tr = table.getElementsByTagName("tr");

    for (i = 1; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[4];

        if (td) {
            txtValue = td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

function newJobContainerVisible() {
    document.getElementById("addJobForm").style.display = "block";
}

async function FetchRequestGET(uri) {
    try {
        const response = await fetch(host + '/' + uri, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            throw new Error('Error performing operation on the Department');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation on the Department');
    }
}
