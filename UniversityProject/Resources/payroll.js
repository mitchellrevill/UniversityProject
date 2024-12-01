const host = `${window.location.protocol}//${window.location.host}`;
const token = localStorage.getItem("authToken");
payload = parseJwt(token)


async function populatePayrollTable() {
    try {

          payroll = {
          "PayrollId": 101,
          "EmployeeId": payload.EmployeeId,
          "TaxNumberCode": "1257L",
          "BaseSalary": 45000.00,
          "ThisPaycheck": 3500.00,
          "Recurrence": "Monthly",
          "Tax": 500.00,
          "ExtraDeductions": 200.00,
          "PayPeriodStart": "2024-11-01T00:00:00",
          "PayPeriodEnd": "2024-11-30T23:59:59",
          "NetPay": 2800.00,
          "Bonuses": 300.00,
          "OvertimeHours": 10,
          "OvertimePay": 150.00,
          "Deductions": ["Health Insurance", "Retirement Fund"],
          "PaymentDate": "2024-11-30T00:00:00",
          "PaymentMethod": "Direct Deposit"
        }

        const Locations = await FetchRequest('GetAllPayrollsById',payroll);
        const tbody = document.getElementById('PayrollTableBody');
        tbody.innerHTML = '';

        if (!Array.isArray(Locations)) {
            throw new Error('Expected an array from FetchRequestGET.');
        }

        if (Locations.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4">No Payrolls available.</td></tr>';
            return;
        }

        console.log(Locations)
        Locations.forEach(item => {
            const row = document.createElement('tr');

            row.innerHTML = `         
            <td>${item.PayrollId}</td>
            <td>${item.EmployeeId}</td>
            <td>${item.PayPeriodStart}</td>
            <td>${item.PayPeriodEnd}</td>
            <td>${item.NetPay}</td>
            `;

            tbody.appendChild(row);
        });
    } catch (error) {
        console.error('Error populating location table:', error);

    }
}

populatePayrollTable()
async function FetchRequest(uri, model) {
    console.log("Req sent");


    const token = localStorage.getItem("authToken");


    let headers = {
        'Content-Type': 'application/json'
    };


    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    try {
        const response = await fetch(host + '/' + uri, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(model)
        });

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            throw new Error('Error performing operation');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation');
        return null;
    }
}
async function FetchRequestGET(uri) {
    try {
        const token = localStorage.getItem("authToken");

        let headers = {
            'Content-Type': 'application/json'
        };

        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        // Make the GET request
        const response = await fetch(host + '/' + uri, {
            method: 'GET',
            headers: headers
        });

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            throw new Error('Error performing operation');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation');
    }
}

function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}