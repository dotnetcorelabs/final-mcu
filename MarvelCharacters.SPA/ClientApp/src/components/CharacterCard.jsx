import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardHeader from '@material-ui/core/CardHeader';
import CardMedia from '@material-ui/core/CardMedia';
import CardContent from '@material-ui/core/CardContent';
import CardActions from '@material-ui/core/CardActions';
import IconButton from '@material-ui/core/IconButton';
import Typography from '@material-ui/core/Typography';
import FavoriteIcon from '@material-ui/icons/Favorite';
import ShareIcon from '@material-ui/icons/Share';
import { red } from '@material-ui/core/colors';

const useStyles = makeStyles({
    card: {
      maxWidth: 345,
    },
    media: {
      height: 140,
    },
    description: {
        height: 40
    },
    header: {
        height: 110
    },
    cardContent: {
        height: 130
    }
  });
  

export default function CharacterCard({character, onLike, onShare}) {
    const classes = useStyles();

    return (
        <Card className={classes.card}>
            {/* <CardActionArea> */}
                <CardMedia
                    className={classes.media}
                    image={character.thumbnail.fullPath}
                    title={character.name}
                />
                <CardContent classes={{root: classes.cardContent}}>
                    <Typography gutterBottom variant="h5" component="h2">
                    {character.name}
                    </Typography>
                    <Typography variant="body2" color="textSecondary" component="p">
                    {character.shortDescription}
                    </Typography>
                </CardContent>
            {/* </CardActionArea> */}
            <CardActions disableSpacing>
                <IconButton aria-label="add to favorites" onClick={() => onLike(character)}>
                    <FavoriteIcon color={character.liked ? "disabled" : "secondary"} />
                </IconButton>
                <IconButton aria-label="share" onClick={() => onShare(character)}>
                    <ShareIcon />
                </IconButton>
            </CardActions>
        </Card>
    );
  }