// ----------------------------------------------------------------------------
// Created By: Jared Fowler
// May 28, 2018
//
// Quest for Glory 1 Save Game creator.
// It was too complicated to find a stable base pointer for cheat engine, and
// the save game file itself was not very comprehensive. Cheat characters were
// created using cheat engine, and the saved game files were copied. This 
// application merely changes the name and skill values in the template files
// and then provides an updated save game directory.
// ----------------------------------------------------------------------------

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// ----------------------------------------------------------------------------

#define uint8_t                     unsigned char
#define uint32_t                    unsigned int

#define HERO_TYPES                  3
#define HERO_NAME_MAX               14
#define BUFFER_SIZE                 65536

#define INPUT_DIR                   "./input/"
#define OUTPUT_URI                  "./output/GLORYSG.%03u"
#define OUTPUT_DIR_URI              "./output/GLORYSG.DIR"

// ----------------------------------------------------------------------------

static char buffer[BUFFER_SIZE];

const char HEROS[HERO_TYPES][10] = { "FIGHTER", "WIZARD", "THIEF" };

// Complete file - Creates a listing of 3 saved games, one for each Hero type.

const unsigned char GLORYSG_DIR[] = { 0x02, 0x00, 0x54, 0x68, 0x69, 0x65, 
  0x66, 0x20, 0x53, 0x74, 0x61, 0x72, 0x74, 0x0A, 0x01, 0x00, 0x57, 0x69, 
  0x7A, 0x61, 0x72, 0x64, 0x20, 0x53, 0x74, 0x61, 0x72, 0x74, 0x0A, 0x00, 
  0x00, 0x46, 0x69, 0x67, 0x68, 0x74, 0x65, 0x72, 0x20, 0x53, 0x74, 0x61, 
  0x72, 0x74, 0x0A, 0xFF, 0xFF };

// ----------------------------------------------------------------------------

int main()
{
    char heroName[HERO_NAME_MAX + 1] = { 0 };
    char tempChar = '0';
    uint32_t i, j;
    FILE* fdIn = NULL;
    FILE* fdOut = NULL;
    size_t readBytes = 0;

    fprintf(stdout, "\r\n\n");
    fprintf(stdout, "Please enter your Hero's name. Max Length: 14. Press ENTER when finished.\r\n");
    fprintf(stdout, "Hero Name: ");

    i = 0;
    while (1)
    {
        tempChar = getchar();

        if (0x0A == tempChar)
        {
            break;
        }
        else if (HERO_NAME_MAX > i)
        {
            heroName[i] = tempChar;
            i++;
        }
    }

    fprintf(stdout, "\r\n\n");
    fprintf(stdout, "Creating files ...\r\n");

    // Read the template file, find and replace the hero name, and export it as a save game file.

    for (j = 0; j < HERO_TYPES; j++)
    {
        sprintf(buffer, "%s%s", INPUT_DIR, HEROS[j]);
        fdIn = fopen(buffer, "rb");
        if (fdIn == NULL)
        {
            fprintf(stderr, "Failed to open input file '%s'\r\n", buffer);
            goto CLEAN_ERROR_EXIT;
        }

        sprintf(buffer, OUTPUT_URI, j);
        fdOut = fopen(buffer, "wb");
        if (fdOut == NULL)
        {
            fprintf(stderr, "Failed to open output file '%s'\r\n", buffer);
            goto CLEAN_ERROR_EXIT;
        }

        readBytes = fread(buffer, sizeof(unsigned char), BUFFER_SIZE, fdIn);

        for (i = 0; i < readBytes; i++)
        {
            if ((buffer[i + 0x00] == 'P') &&
                (buffer[i + 0x01] == 'L') &&
                (buffer[i + 0x02] == 'A') &&
                (buffer[i + 0x03] == 'C') &&
                (buffer[i + 0x04] == 'E') &&
                (buffer[i + 0x05] == 'H') &&
                (buffer[i + 0x06] == 'E') &&
                (buffer[i + 0x07] == 'R') &&
                (buffer[i + 0x08] == 'O') &&
                (buffer[i + 0x09] == 'S') &&
                (buffer[i + 0x0A] == 'N') &&
                (buffer[i + 0x0B] == 'A') &&
                (buffer[i + 0x0C] == 'M') &&
                (buffer[i + 0x0D] == 'E'))
            {
                memcpy(&(buffer[i]), heroName, HERO_NAME_MAX);
                fwrite(buffer, sizeof(unsigned char), readBytes, fdOut);
                break;
            }
        }

        fclose(fdIn);
        fclose(fdOut);
        fdIn = NULL;
        fdOut = NULL;
    }

    // Create Saved Game Directory File

    fdOut = fopen(OUTPUT_DIR_URI, "wb");
    if (fdOut == NULL)
    {
        fprintf(stderr, "Failed to open output file '%s'\r\n", OUTPUT_DIR_URI);
        goto CLEAN_ERROR_EXIT;
    }

    fwrite(GLORYSG_DIR, sizeof(unsigned char), sizeof(GLORYSG_DIR), fdOut);
    fclose(fdOut);
    fdOut = NULL;

    // DONE!

    fprintf(stdout, "Success! Files exported to output folder\r\n\n");

    return 0;

CLEAN_ERROR_EXIT:

    if (fdIn != NULL)
    {
        fclose(fdIn);
        fdIn = NULL;
    }
    if (fdOut != NULL)
    {
        fclose(fdOut);
        fdOut = NULL;
    }

    return 1;
}
