# Set up variables
$testName = "mostbasic.spec.ts" # Replace this with the name of your test
$counterFile = "$PWD\cypress\counter.txt" # Replace this with the name of your counter file
$counter = Get-Content $counterFile

# Convert $counter to an integer
$counter = [int]$counter

# Loop from $counter to 500
for ($i = $counter; $i -le 500; $i++) {
    # Run Cypress with the specified test name
    npx cypress run --spec "cypress/integration/$testName"

    # Increment the counter and save it to the file
    $counter++
    Set-Content -Path $counterFile -Value $counter

    Write-Host "End of iteration " $i
}