// Load employees from storage when the Employees page loads
function loadEmployeesFromStorage() {
    const employeesData = localStorage.getItem('employeesData');

    if (employeesData) {
        const employees = JSON.parse(employeesData);
        displayEmployees(employees); // Call the display function with the parsed data
    } else {
        console.error('No employee data found in localStorage');
        alert('Failed to load employee data. Please try again.');
    }
}

function displayEmployees(employees) {
    const tbody = document.querySelector('#jsonTable tbody');

    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    tbody.innerHTML = ''; // Clear any existing rows

    employees.forEach(employee => {
        const tr = document.createElement('tr');

        // Populate the table row with employee data
        const empIdTd = document.createElement('td');
        empIdTd.innerText = employee.EmployeeId;
        tr.appendChild(empIdTd);

        const firstNameTd = document.createElement('td');
        firstNameTd.innerText = employee.FirstName;
        tr.appendChild(firstNameTd);

        const lastNameTd = document.createElement('td');
        lastNameTd.innerText = employee.LastName;
        tr.appendChild(lastNameTd);

        const companyEmailTd = document.createElement('td');
        companyEmailTd.innerText = employee.CompanyEmail;
        tr.appendChild(companyEmailTd);

        const personalEmailTd = document.createElement('td');
        personalEmailTd.innerText = employee.PersonalEmail;
        tr.appendChild(personalEmailTd);

        tbody.appendChild(tr);
    });
}

// Trigger loading of employees when employeesPage loads
window.addEventListener('load', loadEmployeesFromStorage);



function addNewJob() {

    var postingId = Math.random().toString(36).substring(2, 9); // Generate a random ID
    var jobTitle = document.getElementById("jobTitle").value;
    var jobDescription = document.getElementById("jobDescription").value;
    var jobType = document.getElementById("jobType").value;
    var hours = document.getElementById("hours").value;
    var salary = document.getElementById("salary").value;

    // Validate required fields
    if (!jobTitle || !salary || !jobDescription || !location) {
        alert("You have not answered all required fields");
        return;
    }

    // Define the jobPosting object before using it
    var jobPosting = {
        "postingId": postingId,
        "Title": jobTitle,
        "JobDescription": jobDescription,
        "JobType": jobType,
        "Hours": hours,
        "Salary": salary
    };

    // Use fetch to send the job posting data
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
            alert('Failed to insert job posting');
        });
}



