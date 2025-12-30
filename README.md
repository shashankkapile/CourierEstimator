# Courier Estimator

C#.NET console app to estimate delivery cost and time for given packages with given vehicles with various package and vehicle configurations.
Inputs offers, packages and vehicles.

---

## Features
- **Cost Estimation**  
  Calculates delivery cost based on:
  - Base cost
  - Cost per kilogram
  - Cost per kilometer  
  Applies discounts if a valid offer code is provided.

- **Time Estimation**  
  Uses pick non-pick DP tabulation method to find all possibilities to pick best combination based below conditions
  - Weights, vehicle capacity and shorter distance 
- **Offers**  
  Configurable offers with min/max weight and distance ranges, plus discount percentages.

- **Dependency Injection**  
  Services: OfferService, CostEstimatorService, TimeEstimatorService

---

## Project Structure
- Program.cs: Entry point, handles console input/output, begins estimation.
- Core/Models: POCO Models
- Core/Services: Business logic services:
  - OfferService
  - CostEstimatorService
  - TimeEstimatorService
- Helpers/RoundHelper.cs: Utility for truncating decimals to two places.

---

## Testing
Unit tests are written with xUnit and Moq:
- OfferServiceFacts: Tests valid/invalid offers.
- CostEstimatorServiceFacts: Tests cost calculation with and without offers.
- TimeEstimatorServiceFacts: Ensures correct scheduling and delivery times for various scenarios.
- RoundHelperTests: Tests helper function

---

## Usage
Run the console app and provide input in the following format:
base_delivery_cost no_of_packges
pkg_id1 pkg_weight1_in_kg distance1_in_km offer_code1

....
no_of_vehicles max_speed max_carriable_weight

Example: 
100 5
PKG1 50 30 OFR001
PKG2 75 125 OFFR0008
PKG3 175 100 OFFR003
PKG4 110 60 OFR002
PKG5 155 95 NA
2 70 200
