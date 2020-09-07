RMDIR "Get Costs Stress Test Report" /S /Q
MKDIR "Get Costs Stress Test Report"
del GetCostsStressTestResults.csv
jmeter -H webproxy.services.cig.local -P 8080 -n -t "Get Costs Stress Test Plan.jmx" -l GetCostsStressTestResults.csv -p stress.user.properties -e -o "Get Costs Stress Test Report"

