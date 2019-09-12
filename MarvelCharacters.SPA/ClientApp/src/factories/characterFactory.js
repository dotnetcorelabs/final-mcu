import t from 'prop-types';
import { ThumbnailFactory, thumbnailPropTypesSchema } from './thumbnailFactory';

function getShortDescription(description) {
    if(description)
        return description.substring(0, 36);
    return '';
}

export const CharacterFactory = ({
    id = '',
    name = '',
    description = '',
    resourceURI = '',
    thumbnail = ThumbnailFactory({}),
    liked = false
} = {}) => ({
    id: id,
    name: name,
    description: description,
    shortDescription: getShortDescription(description),
    resourceURI: resourceURI,
    thumbnail: ThumbnailFactory(thumbnail),
    liked: liked
});

export const characterPropTypesSchema = t.shape({
    id: t.string.isRequired,
    name: t.string.isRequired,
    description: t.string,
    resourceURI: t.string,
    thumbnail: thumbnailPropTypesSchema.isRequired
});