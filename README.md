##Personal project PASTI by Manuel Conde Vendrell

This project is done to learn programming on Windows Phone system to have the Pasti App in the all 3 major mobile systems actually (iOS, Android and WP).
It's done with Visual Studio 2012 and for Windows Phone 7.1 or above.

Will cover the next aspects:

- Database (to store all the user pills).
- Internacionalization (translation to EN and ES).
- MVVM model, with Notify properties and Observable Collections.
- Daychange detection (using OnNavigateTo event).

----------------------------------
####Now, the purpose of the program.

I must take some different pills on my current life, but not daily, one can be taken every 2 days, other can be every 3 days, etc. Usually I remember it, because it is every 2 days, but sometimes I forget if I took the pill yesterday. Then I need a way to remember it.

This is the purpose of the program, to view easily if I need to take my pill today or not. Or for various pills.

So the logic is the next: for every pill in the list, knowing the day I started to take the pill and the interval of days to take the next, calculate for today if I must take the pill or not. This is the easy part. The hard part is an screen to show the current week with the days marked for the selected pill to be taked.

With all this in mind, the plan is:

- An starting screen with a list of all the pills for the user and an add button.
- An screen to add the new pills
- An screen to show the current week for the selected pill in which the user can edit the pill data.

Those simple 3 screens have enough work to learn a lot of basic concepts.

----------------------------------
####Screenshots:
![Main](https://raw.github.com/mcvendrell/Pasti_WP7/master/Pasti/Assets/MainScreen.png)
![Screen 1](https://raw.github.com/mcvendrell/Pasti_WP7/master/Pasti/Assets/Add.png)
![Screen 2](https://raw.github.com/mcvendrell/Pasti_WP7/master/Pasti/Assets/IsDay.png)

####License:

I'm using the MIT license, which I find free enough with only the need of attribution to the autor.
