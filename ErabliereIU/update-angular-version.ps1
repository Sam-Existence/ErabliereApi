# a script to update the angular version in the project

# Update the angular version using cli
ng update @angular/cli@16 --force --allow-dirty
ng update @angular/core@16 --force --allow-dirty
ng update @angular-eslint/schematics@16 --force --allow-dirty

npm install ng2-charts chart.js --save